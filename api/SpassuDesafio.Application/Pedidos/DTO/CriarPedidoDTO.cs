using Domain.Entities.Enums;

namespace SpassuDesafio.Application.Pedido.DTO
{
    public record CriarPedidoDTO(string ClienteNome, IEnumerable<CriarItemDTO> Itens);
}
