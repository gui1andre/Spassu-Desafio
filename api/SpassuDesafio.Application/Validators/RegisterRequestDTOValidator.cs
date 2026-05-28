using FluentValidation;
using SpassuDesafio.Application.Auth.DTOs;

namespace SpassuDesafio.Application.Validators;

public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado é inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("As senhas não conferem.");
    }
}
