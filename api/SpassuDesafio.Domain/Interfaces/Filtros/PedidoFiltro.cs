using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpassuDesafio.Domain.Interfaces.Filtros
{
    public record PedidoFiltro(
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
