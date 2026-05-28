using FluentValidation;
using SpassuDesafio.Application.Auth.DTOs;

namespace SpassuDesafio.Application.Validators;

public class LoginRequestDTOValidator : AbstractValidator<LoginRequestDTO>
{
    public LoginRequestDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado é inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}
