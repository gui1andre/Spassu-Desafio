using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpassuDesafio.Application.Pedido.DTO
{
    public record FiltroPedidoDTO(
            string? ClienteNome,
            DateTime? DataInicio,
            DateTime? DataFim,
            decimal ValorMinimo,
            decimal ValorMaximo,
            PedidoStatusEnum? Status,
            int Pagina = 1,
            int TamanhoPagina = 20)
    {
    }
}
