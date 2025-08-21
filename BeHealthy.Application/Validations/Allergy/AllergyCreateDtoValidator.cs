using BeHealthy.Application.Dtos.Allergy;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Allergy;

public class AllergyCreateDtoValidator : AbstractValidator<AllergyCreateDto>
{
    public AllergyCreateDtoValidator()
    {
        RuleFor(a => a.AllergyName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Allergy));

        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Patient));
    }
}
