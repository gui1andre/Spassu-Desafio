using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpassuDesafio.Application.Pedido.DTO;
using SpassuDesafio.Application.Pedido.Interfaces;

namespace SpassuDesafio.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> CriarPedido(CriarPedidoDTO request)
        {
            var pedido = await _pedidoService.CriarPedidoAsync(request);

            return CreatedAtAction(nameof(ObterPedido), new { id = pedido.PedidoId }, pedido);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> ObterPedido(Guid id)
        {
            var pedido = await _pedidoService.ObterPedidoPorIdAsync(id);

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> ObterPedidos([FromQuery] FiltroPedidoDTO filtro)
        {
            var pedidos = await _pedidoService.ObterPedidoAsync(filtro);
            return Ok(pedidos);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarPedido(Guid id)
        {
            var result = await _pedidoService.CancelarPedidoAsync(id);

            if (result == null)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/pagar")]
        public async Task<IActionResult> PagarPedido(Guid id)
        {
            var pedido = await _pedidoService.FecharPedidoPagoAsync(id);

            if (pedido == null)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{pedidoId}/itens")]
        public async Task<ActionResult<ItemPedidoDTO>> AdicionarItem(Guid pedidoId, CriarItemDTO criarItemDTO)
        {
            var item = await _pedidoService.AdicionarItemAsync(pedidoId, criarItemDTO);

            if (item == null)
                return NotFound();

            return CreatedAtAction(nameof(ObterPedido), new { id = pedidoId }, item);
        }

        [HttpDelete("{pedidoId}/itens/{itemId}")]
        public async Task<IActionResult> RemoverItem(Guid pedidoId, Guid itemId)
        {
            await _pedidoService.RemoverItemAsync(pedidoId, itemId);
            return NoContent();
        }

        [HttpPut("{pedidoId}/itens/{itemId}")]
        public async Task<ActionResult<ItemPedidoDTO>> AtualizarItem(Guid pedidoId, Guid itemId, AtualizarItemDTO atualizarItemDTO)
        {
            var item = await _pedidoService.AtualizarItemAsync(pedidoId, itemId, atualizarItemDTO);

            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }
}
