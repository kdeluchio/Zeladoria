using FluentValidation;
using ServiceAuth.Application.Models;

namespace ServiceAuth.Application.Validators;

public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token é obrigatório");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Nova senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$")
            .WithMessage("Senha deve conter pelo menos uma letra maiúscula, uma minúscula e um número");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirmação de senha é obrigatória")
            .Equal(x => x.NewPassword).WithMessage("Senhas não coincidem");
    }
} 