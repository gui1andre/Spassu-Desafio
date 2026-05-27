using System;
using System.Collections.Generic;
using System.Text;

namespace SpassuDesafio.Application.Pedido.DTO
{
    public record AtualizarItemDTO(string ProdutoNome, int Quantidade, decimal PrecoUnitario);
}
