using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Frontend.Validations.Doctor;

public class DoctorForUpdateDtoValidator : AbstractValidator<DoctorUpdateDto>
{
    public DoctorForUpdateDtoValidator(bool requiredSpecialty)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.FirstName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.LastName));

        RuleFor(x => x.PhoneNumber)
           .Matches(@"^\+?[1-9]\d{1,14}$")
           .WithMessage(string.Format(Resource.PropertyInvalidFormat, Resource.PhoneNumber))
           .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        When(_ => requiredSpecialty, () =>
        {
            RuleFor(x => x.SpecialtyId)
                .NotNull()
                .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Specialty));
        });
    }
}
