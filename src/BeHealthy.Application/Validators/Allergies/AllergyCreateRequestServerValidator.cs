using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Allergies;

public class AllergyCreateRequestServerValidator : AbstractValidator<AllergyCreateRequest>
{
    public AllergyCreateRequestServerValidator(IPatientRepository patients)
    {
        RuleFor(x => x.PatientId).MustExist(patients, Resource.Patient);
    }
}
