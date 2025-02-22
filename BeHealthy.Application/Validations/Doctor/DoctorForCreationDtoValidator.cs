using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Doctor;

public class DoctorForCreationDtoValidator : AbstractValidator<DoctorForCreationDto>
{
    public DoctorForCreationDtoValidator(bool requiredSpecialty)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Doctor));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Doctor));

        When(_ => requiredSpecialty, () =>
        {
            RuleFor(x => x.SpecialtyId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Specialty));
        });
    }
}
