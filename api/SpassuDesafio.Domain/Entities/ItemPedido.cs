using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ItemPedido : BaseEntity
    {
        public Guid PedidoId { get; private set; }
        public string ProdutoNome { get; private set; } = string.Empty;
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }


        private ItemPedido() : base() { }


        public ItemPedido(string produtoNome, int quantidade, decimal precoUnitario)
        {
            AtualizarItemPedido(produtoNome, quantidade, precoUnitario);
        }

        public void AtualizarItemPedido(string produtoNome, int quantidade, decimal precoUnitario)
        {
            ValidarNomeProduto(produtoNome);
            ValidarProdutoQuantidade(quantidade);
            ValidarPrecoUnitario(precoUnitario);

            AtualizadoEm = DateTime.UtcNow;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }



        public void AlterarPrecoUnitario(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O preço unitario do produto deve ser maior que 0");

            PrecoUnitario = valor;
            AtualizadoEm = DateTime.UtcNow;
        }

        public void AlterarProdutoNome(string nome)
        {
            ValidarNomeProduto(nome);

            ProdutoNome = nome;

            AtualizadoEm = DateTime.UtcNow;
        }
        public void AlterarProdutoQuantidade(int quantidade)
        {
            ValidarProdutoQuantidade(quantidade);
            Quantidade = quantidade;
            AtualizadoEm = DateTime.UtcNow;
        }

        private static void ValidarNomeProduto(string produtoNome)
        {
            if (string.IsNullOrEmpty(produtoNome) || produtoNome.Length < 2 || produtoNome.Length > 40)
                throw new ArgumentException("O nome do produto deve ter entre 2 e 40 caracteres");

        }
        private static void ValidarPrecoUnitario(decimal precoUnitario)
        {
            if (precoUnitario <= 0)
                throw new ArgumentException("O preço unitario do produto deve ser maior que 0");
        }

        private static void ValidarProdutoQuantidade(int quantidade)
        {
            if (quantidade < 1)
                throw new ArgumentException("O produto deve ter pelo menos 1 unidade");
        }

    }
}
