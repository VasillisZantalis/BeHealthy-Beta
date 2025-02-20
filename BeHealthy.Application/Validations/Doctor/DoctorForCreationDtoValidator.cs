using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Doctor;

public class DoctorForCreationDtoValidator : AbstractValidator<DoctorForCreationDto>
{
    public DoctorForCreationDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Doctor));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Doctor));
    }
}
