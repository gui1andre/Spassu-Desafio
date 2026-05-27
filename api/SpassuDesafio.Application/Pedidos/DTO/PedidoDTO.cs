using Domain.Entities.Enums;

namespace SpassuDesafio.Application.Pedido.DTO
{
    public record PedidoDTO(Guid PedidoId, string ClienteNome, PedidoStatusEnum Status, decimal ValorTotal, IEnumerable<ItemPedidoDTO> Itens, DateTime CriadoEm)
    {
    }
}
