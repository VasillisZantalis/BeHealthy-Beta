using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Frontend.Validations.Patient;

public class PatientForUpdateDtoValidator : AbstractValidator<PatientUpdateDto>
{
    public PatientForUpdateDtoValidator()
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
    }
}
