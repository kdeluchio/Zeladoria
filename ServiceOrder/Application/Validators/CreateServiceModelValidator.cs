using FluentValidation;
using ServiceOrder.Application.Models;

namespace ServiceOrder.Application.Validators;

public class CreateServiceModelValidator : AbstractValidator<CreateServiceModel>
{
    public CreateServiceModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(2, 100)
            .WithMessage("Nome deve ter entre 2 e 100 caracteres");
    }
} 