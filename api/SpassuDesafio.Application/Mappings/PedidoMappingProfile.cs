using AutoMapper;
using Domain.Entities;
using DomainPedido = Domain.Entities.Pedido;
using SpassuDesafio.Application.Pedido.DTO;
using SpassuDesafio.Domain.Interfaces.Filtros;

namespace SpassuDesafio.Application.Mappings
{
    public class PedidoMappingProfile : Profile
    {
        public PedidoMappingProfile()
        {
            CreateMap<DomainPedido, PedidoDTO>()
                .ForCtorParam("PedidoId", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("ClienteNome", opt => opt.MapFrom(src => src.ClienteNome))
                .ForCtorParam("Status", opt => opt.MapFrom(src => src.Status))
                .ForCtorParam("ValorTotal", opt => opt.MapFrom(src => src.ValorTotal))
                .ForCtorParam("Itens", opt => opt.MapFrom(src => src.Itens))
                .ForCtorParam("CriadoEm", opt => opt.MapFrom(src => src.CriadoEm));

            CreateMap<PedidoDTO, DomainPedido>()
                .ConstructUsing((src, context) => new DomainPedido(
                    src.ClienteNome,
                    context.Mapper.Map<List<ItemPedido>>(src.Itens)));

            CreateMap<ItemPedido, ItemPedidoDTO>()
                .ForCtorParam("precoUnitario", opt => opt.MapFrom(src => src.PrecoUnitario));

            CreateMap<ItemPedidoDTO, ItemPedido>()
                .ConstructUsing(src => new ItemPedido(
                    src.ProdutoNome,
                    src.Quantidade,
                    src.PrecoUnitario));

            CreateMap<PedidoFiltro, FiltroPedidoDTO>();
            CreateMap<FiltroPedidoDTO, PedidoFiltro>();
        }
    }
}
