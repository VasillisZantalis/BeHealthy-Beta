using BeHealthy.Shared.Dtos.Specialty;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Frontend.Validations.Specialty;

public class SpecialtyDtoValidator : AbstractValidator<SpecialtyDto>
{
    public SpecialtyDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Name));
    }
}
