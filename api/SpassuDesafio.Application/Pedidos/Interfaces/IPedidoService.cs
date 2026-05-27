using System;
using System.Collections.Generic;
using System.Text;
using SpassuDesafio.Application.Pedido.DTO;

namespace SpassuDesafio.Application.Pedido.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDTO> CriarPedidoAsync(CriarPedidoDTO criarPedidoDTO);
        Task<PedidoDTO?> ObterPedidoPorIdAsync(Guid id);
        Task<List<PedidoDTO>> ObterPedidoAsync(FiltroPedidoDTO filtroPedidoDTO);
        Task<PedidoDTO> FecharPedidoPagoAsync(Guid id);
        Task<PedidoDTO> CancelarPedidoAsync(Guid id);
        Task<ItemPedidoDTO> AdicionarItemAsync(Guid pedidoId, CriarItemDTO criarItemDTO);
        Task<ItemPedidoDTO> AtualizarItemAsync(Guid pedidoId, Guid itemId, AtualizarItemDTO atualizarItemDTO);
        Task RemoverItemAsync(Guid pedidoId, Guid itemId);
    }
}
