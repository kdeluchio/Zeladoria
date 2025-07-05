using FluentValidation;
using ServiceOrder.Application.Models;

namespace ServiceOrder.Application.Validators;

public class CreateOrderModelValidator : AbstractValidator<CreateOrderModel>
{
    public CreateOrderModelValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId deve ser maior que zero");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage("ServiceId deve ser maior que zero");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Descrição é obrigatória")
            .MaximumLength(500)
            .WithMessage("Descrição deve ter no máximo 500 caracteres");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Endereço é obrigatório")
            .MaximumLength(200)
            .WithMessage("Endereço deve ter no máximo 200 caracteres");

        RuleFor(x => x.NumberAddress)
            .NotEmpty()
            .WithMessage("Número do endereço é obrigatório")
            .MaximumLength(20)
            .WithMessage("Número do endereço deve ter no máximo 20 caracteres");

    }
} 