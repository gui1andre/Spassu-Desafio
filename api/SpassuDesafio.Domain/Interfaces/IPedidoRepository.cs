using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SpassuDesafio.Domain.Interfaces.Filtros;

namespace SpassuDesafio.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task CriarAsync(Pedido pedido);
        Task<IEnumerable<Pedido>> ObterAsync(PedidoFiltro filtro);
        Task<Pedido?> ObterPorIdAsync(Guid id);
        Task AtualizarAsync(Pedido pedido);
        Task RemoverAsync(Pedido pedido);

    }
}
