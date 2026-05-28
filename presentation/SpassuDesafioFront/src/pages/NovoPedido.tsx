import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { pedidoService } from '../services/pedidos';
import type { ItemDTO } from '../types/api';

interface ItemFormState {
  produtoNome: string;
  quantidade: string;
  precoUnitario: string;
}

const emptyItem = (): ItemFormState => ({
  produtoNome: '',
  quantidade: '1',
  precoUnitario: '',
});

export function NovoPedido() {
  const [clienteNome, setClienteNome] = useState('');
  const [itens, setItens] = useState<ItemFormState[]>([emptyItem()]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const updateItem = (index: number, field: keyof ItemFormState, value: string) => {
    setItens(prev => prev.map((item, i) => (i === index ? { ...item, [field]: value } : item)));
  };

  const addItem = () => setItens(prev => [...prev, emptyItem()]);

  const removeItem = (index: number) => {
    if (itens.length === 1) return;
    setItens(prev => prev.filter((_, i) => i !== index));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    const parsedItens: ItemDTO[] = itens.map(item => ({
      produtoNome: item.produtoNome.trim(),
      quantidade: Number(item.quantidade),
      precoUnitario: Number(item.precoUnitario),
    }));

    const invalid = parsedItens.find(
      item => !item.produtoNome || item.quantidade <= 0 || item.precoUnitario <= 0
    );
    if (invalid) {
      setError('Preencha todos os campos dos itens corretamente.');
      return;
    }

    setLoading(true);
    try {
      const pedido = await pedidoService.criar({ clienteNome: clienteNome.trim(), itens: parsedItens });
      navigate(`/pedidos/${pedido.pedidoId}`);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao criar pedido');
    } finally {
      setLoading(false);
    }
  };

  const totalEstimado = itens.reduce(
    (acc, item) => acc + (Number(item.quantidade) || 0) * (Number(item.precoUnitario) || 0),
    0
  );

  return (
    <div className="page">
      <div className="page-header">
        <h1>Novo Pedido</h1>
        <button onClick={() => navigate('/pedidos')} className="btn-outline">
          ← Voltar
        </button>
      </div>

      <div className="card">
        <form onSubmit={handleSubmit} className="form">
          <div className="form-group">
            <label htmlFor="clienteNome">Nome do cliente *</label>
            <input
              id="clienteNome"
              type="text"
              value={clienteNome}
              onChange={e => setClienteNome(e.target.value)}
              required
              placeholder="Ex: João Silva"
              style={{ maxWidth: 400 }}
            />
          </div>

          <div className="items-section">
            <div className="items-header">
              <h3>Itens do pedido</h3>
              <button type="button" onClick={addItem} className="btn-outline btn-sm">
                + Adicionar item
              </button>
            </div>

            {itens.map((item, index) => (
              <div key={index} className="item-row">
                <div className="form-group">
                  <label>Produto</label>
                  <input
                    type="text"
                    value={item.produtoNome}
                    onChange={e => updateItem(index, 'produtoNome', e.target.value)}
                    required
                    placeholder="Nome do produto"
                  />
                </div>
                <div className="form-group">
                  <label>Qtd</label>
                  <input
                    type="number"
                    min="1"
                    value={item.quantidade}
                    onChange={e => updateItem(index, 'quantidade', e.target.value)}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Preço unit. (R$)</label>
                  <input
                    type="number"
                    min="0.01"
                    step="0.01"
                    value={item.precoUnitario}
                    onChange={e => updateItem(index, 'precoUnitario', e.target.value)}
                    required
                    placeholder="0,00"
                  />
                </div>
                <button
                  type="button"
                  onClick={() => removeItem(index)}
                  className="btn-danger btn-sm item-remove"
                  disabled={itens.length === 1}
                  title="Remover item"
                >
                  ✕
                </button>
              </div>
            ))}

            <div className="total-row">
              <span>Total estimado</span>
              <strong>
                {totalEstimado.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
              </strong>
            </div>
          </div>

          {error && <p className="error-msg">{error}</p>}

          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={loading}>
              {loading ? 'Criando...' : 'Criar pedido'}
            </button>
            <button type="button" onClick={() => navigate('/pedidos')} className="btn-outline">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
