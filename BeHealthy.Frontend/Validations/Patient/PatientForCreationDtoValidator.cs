using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Frontend.Validations.Patient;

public class PatientForCreationDtoValidator : AbstractValidator<PatientCreateDto>
{
    public PatientForCreationDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.FirstName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.LastName));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Email))
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage(string.Format(Resource.InvalidEmailFormat, Resource.Email));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Password))
            .MinimumLength(6)
            .WithMessage(string.Format(Resource.PasswordTooShort, 6))
            .Matches(@"[A-Z]").WithMessage(Resource.PasswordNeedsUppercase)
            .Matches(@"[a-z]").WithMessage(Resource.PasswordNeedsLowercase)
            .Matches(@"[\W_]").WithMessage(Resource.PasswordNeedsNonAlphanumericCharacter)
            .Matches(@"[\d]").WithMessage(Resource.PasswordNeedsDigit);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.ConfirmPassword))
            .Equal(x => x.Password)
            .WithMessage(Resource.PasswordsDoNotMatch);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage(string.Format(Resource.PropertyInvalidFormat, Resource.PhoneNumber))
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}
