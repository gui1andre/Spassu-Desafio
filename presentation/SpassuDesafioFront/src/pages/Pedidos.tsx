import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { pedidoService } from '../services/pedidos';
import { PedidoStatus } from '../types/api';
import type { PedidoDTO, PedidoFiltros } from '../types/api';

const STATUS_LABELS: Record<number, string> = {
  [PedidoStatus.Novo]: 'Novo',
  [PedidoStatus.Pago]: 'Pago',
  [PedidoStatus.Cancelado]: 'Cancelado',
};

const STATUS_CLASS: Record<number, string> = {
  [PedidoStatus.Novo]: 'badge-novo',
  [PedidoStatus.Pago]: 'badge-pago',
  [PedidoStatus.Cancelado]: 'badge-cancelado',
};

interface FiltroForm {
  clienteNome: string;
  status: string;
  dataInicio: string;
  dataFim: string;
  valorMinimo: string;
  valorMaximo: string;
}

const filtroVazio: FiltroForm = {
  clienteNome: '',
  status: '',
  dataInicio: '',
  dataFim: '',
  valorMinimo: '',
  valorMaximo: '',
};

const PAGE_SIZE = 10;

function formToFiltros(form: FiltroForm, pagina: number): PedidoFiltros {
  return {
    ...(form.clienteNome ? { clienteNome: form.clienteNome } : {}),
    ...(form.status !== '' ? { status: Number(form.status) } : {}),
    ...(form.dataInicio ? { dataInicio: form.dataInicio } : {}),
    ...(form.dataFim ? { dataFim: form.dataFim } : {}),
    ...(form.valorMinimo ? { valorMinimo: Number(form.valorMinimo) } : {}),
    ...(form.valorMaximo ? { valorMaximo: Number(form.valorMaximo) } : {}),
    pagina,
    tamanhoPagina: PAGE_SIZE,
  };
}

export function Pedidos() {
  const [pedidos, setPedidos] = useState<PedidoDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [pagina, setPagina] = useState(1);
  const [filtroAtivo, setFiltroAtivo] = useState<FiltroForm>(filtroVazio);
  const [form, setForm] = useState<FiltroForm>(filtroVazio);
  const navigate = useNavigate();

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError('');

    pedidoService
      .listar(formToFiltros(filtroAtivo, pagina))
      .then(data => {
        if (!cancelled) setPedidos(data);
      })
      .catch(err => {
        if (!cancelled) setError(err instanceof Error ? err.message : 'Erro ao carregar pedidos');
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => { cancelled = true; };
  }, [filtroAtivo, pagina]);

  const handleFiltrar = (e: React.FormEvent) => {
    e.preventDefault();
    setPagina(1);
    setFiltroAtivo(form);
  };

  const handleLimpar = () => {
    setForm(filtroVazio);
    setPagina(1);
    setFiltroAtivo(filtroVazio);
  };

  const formatCurrency = (v: number) =>
    v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

  const formatDate = (d: string) =>
    new Date(d).toLocaleDateString('pt-BR');

  return (
    <div className="page">
      <div className="page-header">
        <h1>Pedidos</h1>
        <button onClick={() => navigate('/pedidos/novo')} className="btn-primary">
          + Novo Pedido
        </button>
      </div>

      <div className="filter-card">
        <form onSubmit={handleFiltrar} className="filter-form">
          <div className="filter-grid">
            <div className="form-group">
              <label>Cliente</label>
              <input
                type="text"
                value={form.clienteNome}
                onChange={e => setForm(f => ({ ...f, clienteNome: e.target.value }))}
                placeholder="Nome do cliente"
              />
            </div>
            <div className="form-group">
              <label>Status</label>
              <select
                value={form.status}
                onChange={e => setForm(f => ({ ...f, status: e.target.value }))}
              >
                <option value="">Todos</option>
                <option value="0">Novo</option>
                <option value="1">Pago</option>
                <option value="2">Cancelado</option>
              </select>
            </div>
            <div className="form-group">
              <label>Data início</label>
              <input
                type="date"
                value={form.dataInicio}
                onChange={e => setForm(f => ({ ...f, dataInicio: e.target.value }))}
              />
            </div>
            <div className="form-group">
              <label>Data fim</label>
              <input
                type="date"
                value={form.dataFim}
                onChange={e => setForm(f => ({ ...f, dataFim: e.target.value }))}
              />
            </div>
            <div className="form-group">
              <label>Valor mínimo</label>
              <input
                type="number"
                min="0"
                step="0.01"
                value={form.valorMinimo}
                onChange={e => setForm(f => ({ ...f, valorMinimo: e.target.value }))}
                placeholder="0,00"
              />
            </div>
            <div className="form-group">
              <label>Valor máximo</label>
              <input
                type="number"
                min="0"
                step="0.01"
                value={form.valorMaximo}
                onChange={e => setForm(f => ({ ...f, valorMaximo: e.target.value }))}
                placeholder="0,00"
              />
            </div>
          </div>
          <div className="filter-actions">
            <button type="submit" className="btn-primary">Filtrar</button>
            <button type="button" onClick={handleLimpar} className="btn-outline">
              Limpar
            </button>
          </div>
        </form>
      </div>

      {error && <p className="error-msg">{error}</p>}

      <div className="table-card">
        {loading ? (
          <p className="loading-text">Carregando...</p>
        ) : pedidos.length === 0 ? (
          <p className="empty-text">Nenhum pedido encontrado.</p>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Cliente</th>
                <th>Status</th>
                <th>Valor total</th>
                <th>Data</th>
                <th>Itens</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {pedidos.map(p => (
                <tr key={p.pedidoId}>
                  <td>{p.clienteNome}</td>
                  <td>
                    <span className={`badge ${STATUS_CLASS[p.status]}`}>
                      {STATUS_LABELS[p.status]}
                    </span>
                  </td>
                  <td>{formatCurrency(p.valorTotal)}</td>
                  <td>{formatDate(p.criadoEm)}</td>
                  <td>{p.itens.length}</td>
                  <td>
                    <button
                      className="btn-link"
                      onClick={() => navigate(`/pedidos/${p.pedidoId}`)}
                    >
                      Ver detalhes
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      <div className="pagination">
        <button
          onClick={() => setPagina(p => Math.max(1, p - 1))}
          disabled={pagina === 1 || loading}
          className="btn-outline"
        >
          Anterior
        </button>
        <span>Página {pagina}</span>
        <button
          onClick={() => setPagina(p => p + 1)}
          disabled={pedidos.length < PAGE_SIZE || loading}
          className="btn-outline"
        >
          Próxima
        </button>
      </div>
    </div>
  );
}
