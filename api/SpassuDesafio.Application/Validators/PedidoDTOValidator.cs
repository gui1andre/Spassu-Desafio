using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using SpassuDesafio.Application.Pedido.DTO;

namespace SpassuDesafio.Application.Validators
{
    public class PedidoDTOValidator : AbstractValidator<PedidoDTO>
    {
        public PedidoDTOValidator()
        {
            RuleFor(x => x.ClienteNome)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                .MinimumLength(5)
                .MaximumLength(50).WithMessage("O nome do cliente deve ter entre 5 e 50 caracteres.");
            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
                .ForEach(item =>
                {
                    item.SetValidator(new ItemPedidoDTOValidator());
                });
        }
    }
}