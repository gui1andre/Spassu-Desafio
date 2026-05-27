using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SpassuDesafio.Domain.Interfaces;
using SpassuDesafio.Domain.Interfaces.Filtros;
using SpassuDesafio.Infrastructure.Data;

namespace SpassuDesafio.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarAsync(Pedido pedido)
        {
            await _context.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Pedido>> ObterAsync(PedidoFiltro filtro)
        {
            var pagina = filtro.Pagina < 1 ? 1 : filtro.Pagina;
            var tamanhoPagina = filtro.TamanhoPagina < 1 ? 20 : Math.Min(filtro.TamanhoPagina, 100);

            var query = _context.Pedidos.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.ClienteNome))
            {
                query = query.Where(p => p.ClienteNome.Contains(filtro.ClienteNome));
            }

            if (filtro.DataInicio.HasValue)
            {
                query = query.Where(p => p.CriadoEm >= filtro.DataInicio.Value);
            }

            if (filtro.DataFim.HasValue)
            {
                query = query.Where(p => p.CriadoEm <= filtro.DataFim.Value);
            }

            if (filtro.ValorMinimo > 0)
            {
                query = query.Where(p => p.ValorTotal >= filtro.ValorMinimo);
            }

            if (filtro.ValorMaximo > 0)
            {
                query = query.Where(p => p.ValorTotal <= filtro.ValorMaximo);
            }

            if(filtro.Status.HasValue)
            {
                query = query.Where(p => p.Status == filtro.Status.Value);
            }

            var skip = (pagina - 1) * tamanhoPagina;

            return await query
                .AsNoTracking()
                .Include(x => x.Itens)
                .OrderByDescending(x => x.CriadoEm)
                .Skip(skip)
                .Take(tamanhoPagina)
                .ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pedidos.Include(x => x.Itens).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Pedido pedido)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
