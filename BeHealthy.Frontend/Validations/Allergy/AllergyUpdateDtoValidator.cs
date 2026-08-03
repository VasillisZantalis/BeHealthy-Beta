using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Frontend.Validations.Allergy;

public class AllergyUpdateDtoValidator : AbstractValidator<AllergyUpdateRequest>
{
    public AllergyUpdateDtoValidator()
    {
        RuleFor(a => a.AllergyName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Allergy));

        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.PropertyRequired, Resource.Patient));
    }
}
