export const PedidoStatus = {
  Novo: 0,
  Pago: 1,
  Cancelado: 2,
} as const;

export type PedidoStatus = (typeof PedidoStatus)[keyof typeof PedidoStatus];

export interface ItemPedidoDTO {
  id: string;
  pedidoId: string;
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
  criadoEm: string;
}

export interface PedidoDTO {
  pedidoId: string;
  clienteNome: string;
  status: PedidoStatus;
  valorTotal: number;
  itens: ItemPedidoDTO[];
  criadoEm: string;
}

export interface AuthResponse {
  token: string;
  expiration: string;
}

export interface PedidoFiltros {
  clienteNome?: string;
  dataInicio?: string;
  dataFim?: string;
  valorMinimo?: number;
  valorMaximo?: number;
  status?: number;
  pagina?: number;
  tamanhoPagina?: number;
}

export interface CriarPedidoDTO {
  clienteNome: string;
  itens: ItemDTO[];
}

export interface ItemDTO {
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
}
