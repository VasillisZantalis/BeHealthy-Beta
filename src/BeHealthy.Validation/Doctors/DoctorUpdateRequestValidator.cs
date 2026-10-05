using BeHealthy.Shared.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Validation.Settings;

namespace BeHealthy.Validation.Doctors;

public class DoctorUpdateRequestValidator : AbstractValidator<DoctorUpdateRequest>
{
    public DoctorUpdateRequestValidator(IValidationSettingsProvider settings)
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.FirstName).PersonName(Resource.FirstName);
        RuleFor(x => x.LastName).PersonName(Resource.LastName);
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
