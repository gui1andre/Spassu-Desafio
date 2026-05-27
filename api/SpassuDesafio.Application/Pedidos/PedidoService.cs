using AutoMapper;
using DomainPedido = Domain.Entities.Pedido;
using SpassuDesafio.Application.Pedido.DTO;
using SpassuDesafio.Application.Pedido.Interfaces;
using SpassuDesafio.Domain.Interfaces;
using Domain.Entities;
using SpassuDesafio.Domain.Interfaces.Filtros;
using Domain.Entities.Enums;

namespace SpassuDesafio.Application.Pedidos
{
    internal class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository pedidoRepository, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task<PedidoDTO> CriarPedidoAsync(CriarPedidoDTO criarPedidoDTO)
        {
            var itens = criarPedidoDTO.Itens
                .Select(i => new ItemPedido(i.ProdutoNome, i.Quantidade, i.PrecoUnitario))
                .ToList();

            var pedido = new DomainPedido(criarPedidoDTO.ClienteNome, itens);

            await _pedidoRepository.CriarAsync(pedido);

            return _mapper.Map<PedidoDTO>(pedido);
        }

        public async Task<List<PedidoDTO>> ObterPedidoAsync(FiltroPedidoDTO filtroPedidoDTO)
        {
            var filtro = _mapper.Map<PedidoFiltro>(filtroPedidoDTO);
            var pedidos = await _pedidoRepository.ObterAsync(filtro);
            return _mapper.Map<List<PedidoDTO>>(pedidos);
        }

        public async Task<PedidoDTO?> ObterPedidoPorIdAsync(Guid id)
        {
            var pedido = await ObterPedidoOrthrowAsync(id);

            return _mapper.Map<PedidoDTO>(pedido);
        }
        public async Task<PedidoDTO> FecharPedidoPagoAsync(Guid id)
        {
            var pedido = await ObterPedidoOrthrowAsync(id);
            pedido.AtualizarStatus(PedidoStatusEnum.Pago);
            await _pedidoRepository.AtualizarAsync(pedido);

            return _mapper.Map<PedidoDTO>(pedido);
        }



        public async Task<PedidoDTO> CancelarPedidoAsync(Guid id)
        {
            var pedido = await ObterPedidoOrthrowAsync(id);

            pedido.AtualizarStatus(PedidoStatusEnum.Cancelado);

            await _pedidoRepository.AtualizarAsync(pedido);
            return _mapper.Map<PedidoDTO>(pedido);
        }

        public async Task<ItemPedidoDTO> AdicionarItemAsync(Guid pedidoId, CriarItemDTO criarItemDTO)
        {
            var pedido = await ObterPedidoOrthrowAsync(pedidoId);
            var item = new ItemPedido(criarItemDTO.ProdutoNome, criarItemDTO.Quantidade, criarItemDTO.PrecoUnitario);
                        pedido.AdicionarItem(item);

            await _pedidoRepository.AtualizarAsync(pedido);
            return _mapper.Map<ItemPedidoDTO>(item);
        }

        public async Task<ItemPedidoDTO> AtualizarItemAsync(Guid pedidoId, Guid itemId, AtualizarItemDTO atualizarItemDTO)
        {
            var pedido = await ObterPedidoOrthrowAsync(pedidoId);

            var item = pedido.Itens.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new KeyNotFoundException("Não foi encontrado um item com o ID fornecido.");

            pedido.AtualizarItem(item.Id, atualizarItemDTO.ProdutoNome, atualizarItemDTO.Quantidade, atualizarItemDTO.PrecoUnitario);
            await _pedidoRepository.AtualizarAsync(pedido);

            return _mapper.Map<ItemPedidoDTO>(item);
        }
        public async Task RemoverItemAsync(Guid pedidoId, Guid itemId)
        {
            var pedido = await ObterPedidoOrthrowAsync(pedidoId);
            var item = pedido.Itens.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                throw new KeyNotFoundException("Não foi encontrado um item com o ID fornecido.");

            pedido.RemoverItem(item.Id);

            await _pedidoRepository.AtualizarAsync(pedido);
             
        }

        private async Task<DomainPedido> ObterPedidoOrthrowAsync(Guid id) 
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id);
            if (pedido == null)
                throw new KeyNotFoundException("Não foi encontrado um pedido com o ID fornecido.");
            return pedido;
        }
    }
}
