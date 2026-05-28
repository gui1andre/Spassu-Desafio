import { api } from './api';
import type { PedidoDTO, PedidoFiltros, CriarPedidoDTO, ItemPedidoDTO, ItemDTO } from '../types/api';

function toQueryString(params: Record<string, unknown>): string {
  const query = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value !== undefined && value !== null && value !== '') {
      query.set(key, String(value));
    }
  }
  const str = query.toString();
  return str ? `?${str}` : '';
}

export const pedidoService = {
  listar: (filtros: PedidoFiltros = {}) =>
    api.get<PedidoDTO[]>(`/Pedido${toQueryString(filtros as Record<string, unknown>)}`),

  buscarPorId: (id: string) =>
    api.get<PedidoDTO>(`/Pedido/${id}`),

  criar: (dados: CriarPedidoDTO) =>
    api.post<PedidoDTO>('/Pedido', dados),

  cancelar: (id: string) =>
    api.put<void>(`/Pedido/${id}/cancelar`),

  pagar: (id: string) =>
    api.put<void>(`/Pedido/${id}/pagar`),

  adicionarItem: (pedidoId: string, item: ItemDTO) =>
    api.post<ItemPedidoDTO>(`/Pedido/${pedidoId}/itens`, item),

  atualizarItem: (pedidoId: string, itemId: string, item: ItemDTO) =>
    api.put<ItemPedidoDTO>(`/Pedido/${pedidoId}/itens/${itemId}`, item),

  removerItem: (pedidoId: string, itemId: string) =>
    api.delete<void>(`/Pedido/${pedidoId}/itens/${itemId}`),
};
