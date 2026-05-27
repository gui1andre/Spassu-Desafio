using Domain.Entities;
using Xunit;

namespace SpassuDesafio.Tests.Domain;

public class ItemPedidoTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void QuantidadeDeveSerMaiorQueZero(int quantidadeInvalida)
    {
        Assert.Throws<ArgumentException>(() => new ItemPedido("Produto A", quantidadeInvalida, 10m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void PrecoUnitarioDeveSerMaiorQueZero(decimal precoInvalido)
    {
        Assert.Throws<ArgumentException>(() => new ItemPedido("Produto A", 1, precoInvalido));
    }
}