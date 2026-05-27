using Domain.Entities;
using Domain.Entities.Enums;
using Xunit;

namespace SpassuDesafio.Tests.Domain;

public class PedidoTests
{
    [Fact]
    public void DeveCriarPedidoComStatusNovoEValorTotalCalculado()
    {

        var itens = new List<ItemPedido>
        {
            new("Produto A", 2, 10m),
            new("Produto B", 1, 15m)
        };

        var pedido = new Pedido("Cliente Válido", itens);

        Assert.Equal(PedidoStatusEnum.Novo, pedido.Status);
        Assert.Equal(35m, pedido.ValorTotal);
        Assert.Equal(2, pedido.Itens.Count);
    }

    [Fact]
    public void NaoDevePermitirPedidoSemItens()
    {
        Assert.Throws<ArgumentException>(() => new Pedido("Cliente Válido", []));
    }

    [Fact]
    public void DeveRecalcularValorTotalAoAdicionarItem()
    {
        var pedido = new Pedido("Cliente Válido", [new ItemPedido("Produto A", 1, 10m)]);

        pedido.AdicionarItem(new ItemPedido("Produto B", 2, 5m));

        Assert.Equal(20m, pedido.ValorTotal);
    }

    [Fact]
    public void DeveRecalcularValorTotalAoRemoverItem()
    {
        var item1 = new ItemPedido("Produto A", 1, 10m);
        var item2 = new ItemPedido("Produto B", 2, 5m);
        var pedido = new Pedido("Cliente Válido", [item1, item2]);

        pedido.RemoverItem(item2.Id);

        Assert.Equal(10m, pedido.ValorTotal);
        Assert.Single(pedido.Itens);
    }

    [Fact]
    public void DeveRecalcularValorTotalAoAtualizarItem()
    {
        var item = new ItemPedido("Produto A", 1, 10m);
        var pedido = new Pedido("Cliente Válido", [item]);

        pedido.AtualizarItem(item.Id, "Produto A", 3, 10m);

        Assert.Equal(30m, pedido.ValorTotal);
    }

    [Fact]
    public void PedidoPagoNaoPodeSerCancelado()
    {
        var pedido = new Pedido("Cliente Válido", [new ItemPedido("Produto A", 1, 10m)]);
        pedido.AtualizarStatus(PedidoStatusEnum.Pago);

        Assert.Throws<InvalidOperationException>(() => pedido.AtualizarStatus(PedidoStatusEnum.Cancelado));
    }

    [Fact]
    public void DeveLancarExcecaoAoAtualizarItemInexistente()
    {
        var pedido = new Pedido("Cliente Válido", [new ItemPedido("Produto A", 1, 10m)]);

        Assert.Throws<ArgumentException>(() =>
            pedido.AtualizarItem(Guid.NewGuid(), "Produto X", 2, 5m));
    }
}