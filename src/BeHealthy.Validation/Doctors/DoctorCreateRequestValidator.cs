using BeHealthy.Shared.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Validation.Settings;

namespace BeHealthy.Validation.Doctors;

public class DoctorCreateRequestValidator : AbstractValidator<DoctorCreateRequest>
{
    public DoctorCreateRequestValidator(IValidationSettingsProvider settings)
    {
        RuleFor(x => x.FirstName).PersonName(Resource.FirstName);
        RuleFor(x => x.LastName).PersonName(Resource.LastName);
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();

        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.ConfirmPassword))
            .Equal(x => x.Password).WithMessage(Resource.PasswordsDoNotMatch);

        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Image).Image();
        RuleFor(x => x.DepartmentId).OptionalReference(Resource.Department);
        RuleFor(x => x.SpecialtyId).OptionalReference(Resource.Specialty);

        WhenAsync((_, cancellationToken) => settings.IsEnabledAsync(AppSettingKeys.DoNotAllowDoctorWithoutSpecialty, cancellationToken), () =>
        {
            RuleFor(x => x.SpecialtyId).NotNull().WithMessage(ValidationMessages.Required(Resource.Specialty));
        });
    }
}
