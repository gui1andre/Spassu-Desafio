import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { pedidoService } from '../services/pedidos';
import { PedidoStatus } from '../types/api';
import type { PedidoDTO, ItemPedidoDTO, ItemDTO } from '../types/api';

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

interface ItemEditState {
  produtoNome: string;
  quantidade: string;
  precoUnitario: string;
}

interface NovoItemState {
  produtoNome: string;
  quantidade: string;
  precoUnitario: string;
}

export function PedidoDetalhe() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [pedido, setPedido] = useState<PedidoDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [actionLoading, setActionLoading] = useState(false);

  const [editingItemId, setEditingItemId] = useState<string | null>(null);
  const [itemEdit, setItemEdit] = useState<ItemEditState>({ produtoNome: '', quantidade: '', precoUnitario: '' });

  const [showAddForm, setShowAddForm] = useState(false);
  const [novoItem, setNovoItem] = useState<NovoItemState>({ produtoNome: '', quantidade: '1', precoUnitario: '' });

  const carregar = async () => {
    if (!id) return;
    try {
      const data = await pedidoService.buscarPorId(id);
      setPedido(data);
      setError('');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao carregar pedido');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    carregar();
  }, [id]);

  const handlePagar = async () => {
    if (!id || !window.confirm('Confirmar pagamento deste pedido?')) return;
    setActionLoading(true);
    try {
      await pedidoService.pagar(id);
      await carregar();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao registrar pagamento');
    } finally {
      setActionLoading(false);
    }
  };

  const handleCancelar = async () => {
    if (!id || !window.confirm('Tem certeza que deseja cancelar este pedido?')) return;
    setActionLoading(true);
    try {
      await pedidoService.cancelar(id);
      await carregar();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao cancelar pedido');
    } finally {
      setActionLoading(false);
    }
  };

  const startEdit = (item: ItemPedidoDTO) => {
    setEditingItemId(item.id);
    setItemEdit({
      produtoNome: item.produtoNome,
      quantidade: String(item.quantidade),
      precoUnitario: String(item.precoUnitario),
    });
  };

  const cancelEdit = () => setEditingItemId(null);

  const saveEdit = async (itemId: string) => {
    if (!id) return;
    const dados: ItemDTO = {
      produtoNome: itemEdit.produtoNome.trim(),
      quantidade: Number(itemEdit.quantidade),
      precoUnitario: Number(itemEdit.precoUnitario),
    };
    try {
      await pedidoService.atualizarItem(id, itemId, dados);
      setEditingItemId(null);
      await carregar();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao atualizar item');
    }
  };

  const handleRemoverItem = async (itemId: string) => {
    if (!id || !window.confirm('Remover este item do pedido?')) return;
    try {
      await pedidoService.removerItem(id, itemId);
      await carregar();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao remover item');
    }
  };

  const handleAdicionarItem = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) return;
    const dados: ItemDTO = {
      produtoNome: novoItem.produtoNome.trim(),
      quantidade: Number(novoItem.quantidade),
      precoUnitario: Number(novoItem.precoUnitario),
    };
    try {
      await pedidoService.adicionarItem(id, dados);
      setNovoItem({ produtoNome: '', quantidade: '1', precoUnitario: '' });
      setShowAddForm(false);
      await carregar();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao adicionar item');
    }
  };

  const formatCurrency = (v: number) =>
    v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });

  const formatDate = (d: string) =>
    new Date(d).toLocaleString('pt-BR');

  if (loading) {
    return (
      <div className="page">
        <p className="loading-text">Carregando...</p>
      </div>
    );
  }

  if (!pedido) {
    return (
      <div className="page">
        <p className="error-msg">{error || 'Pedido não encontrado.'}</p>
        <button onClick={() => navigate('/pedidos')} className="btn-outline" style={{ marginTop: 12 }}>
          ← Voltar para pedidos
        </button>
      </div>
    );
  }

  const isNovo = pedido.status === PedidoStatus.Novo;

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Pedido</h1>
          <p className="pedido-id">#{pedido.pedidoId}</p>
        </div>
        <button onClick={() => navigate('/pedidos')} className="btn-outline">
          ← Voltar
        </button>
      </div>

      {error && <p className="error-msg">{error}</p>}

      <div className="detail-grid">
        <div className="card">
          <h2 className="card-title">Informações</h2>
          <dl className="detail-list">
            <dt>Cliente</dt>
            <dd>{pedido.clienteNome}</dd>
            <dt>Status</dt>
            <dd>
              <span className={`badge ${STATUS_CLASS[pedido.status]}`}>
                {STATUS_LABELS[pedido.status]}
              </span>
            </dd>
            <dt>Valor total</dt>
            <dd>
              <strong>{formatCurrency(pedido.valorTotal)}</strong>
            </dd>
            <dt>Criado em</dt>
            <dd>{formatDate(pedido.criadoEm)}</dd>
          </dl>

          {isNovo && (
            <div className="action-buttons">
              <button
                onClick={handlePagar}
                className="btn-success"
                disabled={actionLoading}
              >
                Confirmar pagamento
              </button>
              <button
                onClick={handleCancelar}
                className="btn-danger"
                disabled={actionLoading}
              >
                Cancelar pedido
              </button>
            </div>
          )}
        </div>

        <div className="card">
          <div className="card-header">
            <h2 className="card-title">Itens ({pedido.itens.length})</h2>
            {isNovo && (
              <button
                onClick={() => {
                  setShowAddForm(v => !v);
                  setNovoItem({ produtoNome: '', quantidade: '1', precoUnitario: '' });
                }}
                className="btn-outline btn-sm"
              >
                {showAddForm ? 'Cancelar' : '+ Adicionar item'}
              </button>
            )}
          </div>

          {showAddForm && (
            <form onSubmit={handleAdicionarItem} className="item-add-form">
              <input
                type="text"
                value={novoItem.produtoNome}
                onChange={e => setNovoItem(v => ({ ...v, produtoNome: e.target.value }))}
                required
                placeholder="Nome do produto"
              />
              <input
                type="number"
                min="1"
                value={novoItem.quantidade}
                onChange={e => setNovoItem(v => ({ ...v, quantidade: e.target.value }))}
                required
                placeholder="Qtd"
              />
              <input
                type="number"
                min="0.01"
                step="0.01"
                value={novoItem.precoUnitario}
                onChange={e => setNovoItem(v => ({ ...v, precoUnitario: e.target.value }))}
                required
                placeholder="Preço (R$)"
              />
              <button type="submit" className="btn-primary btn-sm">
                Adicionar
              </button>
            </form>
          )}

          <table className="table">
            <thead>
              <tr>
                <th>Produto</th>
                <th>Qtd</th>
                <th>Preço unit.</th>
                <th>Subtotal</th>
                {isNovo && <th></th>}
              </tr>
            </thead>
            <tbody>
              {pedido.itens.length === 0 ? (
                <tr>
                  <td colSpan={isNovo ? 5 : 4} className="empty-text">
                    Nenhum item neste pedido.
                  </td>
                </tr>
              ) : (
                pedido.itens.map(item => (
                  <tr key={item.id}>
                    {editingItemId === item.id ? (
                      <>
                        <td>
                          <input
                            type="text"
                            value={itemEdit.produtoNome}
                            onChange={e => setItemEdit(v => ({ ...v, produtoNome: e.target.value }))}
                            className="input-inline"
                          />
                        </td>
                        <td>
                          <input
                            type="number"
                            min="1"
                            value={itemEdit.quantidade}
                            onChange={e => setItemEdit(v => ({ ...v, quantidade: e.target.value }))}
                            className="input-inline input-sm"
                          />
                        </td>
                        <td>
                          <input
                            type="number"
                            min="0.01"
                            step="0.01"
                            value={itemEdit.precoUnitario}
                            onChange={e => setItemEdit(v => ({ ...v, precoUnitario: e.target.value }))}
                            className="input-inline"
                          />
                        </td>
                        <td>
                          {formatCurrency(
                            (Number(itemEdit.quantidade) || 0) * (Number(itemEdit.precoUnitario) || 0)
                          )}
                        </td>
                        <td className="row-actions">
                          <button onClick={() => saveEdit(item.id)} className="btn-link">
                            Salvar
                          </button>
                          <button onClick={cancelEdit} className="btn-link">
                            Cancelar
                          </button>
                        </td>
                      </>
                    ) : (
                      <>
                        <td>{item.produtoNome}</td>
                        <td>{item.quantidade}</td>
                        <td>{formatCurrency(item.precoUnitario)}</td>
                        <td>{formatCurrency(item.quantidade * item.precoUnitario)}</td>
                        {isNovo && (
                          <td className="row-actions">
                            <button onClick={() => startEdit(item)} className="btn-link">
                              Editar
                            </button>
                            <button
                              onClick={() => handleRemoverItem(item.id)}
                              className="btn-link btn-link-danger"
                            >
                              Remover
                            </button>
                          </td>
                        )}
                      </>
                    )}
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
