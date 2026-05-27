using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SpassuDesafio.Application.Pedido.DTO;

namespace SpassuDesafio.Application.Validators
{
    public class ItemPedidoDTOValidator : AbstractValidator<ItemPedidoDTO>
    {
        public ItemPedidoDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do produto é obrigatório.");

            RuleFor(x => x.PedidoId)
                .NotEmpty().WithMessage("O ID do pedido é obrigatório.");

            RuleFor(x => x.ProdutoNome)
               .NotEmpty().WithMessage("O nome do produto é obrigatório.")
               .MinimumLength(2)
               .MaximumLength(40).WithMessage("O nome do produto deve ter entre 2 e 40 caracteres.");
            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
            RuleFor(x => x.PrecoUnitario)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
        }
    }
}
