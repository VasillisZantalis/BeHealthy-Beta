using BeHealthy.Shared.Common;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Validation.Settings;

namespace BeHealthy.Validation.Appointments;

public class AppointmentUpdateRequestValidator : AbstractValidator<AppointmentUpdateRequest>
{
    public AppointmentUpdateRequestValidator(IValidationSettingsProvider settings)
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.PatientId).RequiredReference(Resource.Patient);
        RuleFor(x => x.DoctorId).RequiredReference(Resource.Doctor);
        RuleFor(x => x.RoomId).OptionalReference(Resource.Room);
        RuleFor(x => x.NurseId).OptionalReference(Resource.Nurse);

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.Date));

        RuleFor(x => x.AppointmentEndTime)
            .GreaterThan(x => x.AppointmentStartTime).WithMessage(Resource.EndTimeMustBeLaterThanStartTime);

        RuleFor(x => x.Reason).DefinedEnum(Resource.Reason);
        RuleFor(x => x.Status).DefinedEnum(Resource.Status);
        RuleFor(x => x.Notes).OptionalText(Resource.Notes, FieldLengths.AppointmentNotes);

        WhenAsync((_, cancellationToken) => settings.IsEnabledAsync(AppSettingKeys.AppointmentRequiresRoom, cancellationToken), () =>
        {
            RuleFor(x => x.RoomId).NotNull().WithMessage(ValidationMessages.Required(Resource.Room));
        });

        WhenAsync((_, cancellationToken) => settings.IsEnabledAsync(AppSettingKeys.NurseIsRequiredForAppointment, cancellationToken), () =>
        {
            RuleFor(x => x.NurseId).NotNull().WithMessage(ValidationMessages.Required(Resource.Nurse));
        });
    }
}
