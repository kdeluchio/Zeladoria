using FluentValidation;
using ServiceAuth.Application.Models;

namespace ServiceAuth.Application.Validators;

public class SignupModelValidator : AbstractValidator<SignupModel>
{
    public SignupModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(2).WithMessage("Nome deve ter pelo menos 2 caracteres")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email deve ter um formato válido");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("CPF é obrigatório");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$")
            .WithMessage("Senha deve conter pelo menos uma letra maiúscula, uma minúscula e um número");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirmação de senha é obrigatória")
            .Equal(x => x.Password).WithMessage("Senhas não coincidem");
    }
} 