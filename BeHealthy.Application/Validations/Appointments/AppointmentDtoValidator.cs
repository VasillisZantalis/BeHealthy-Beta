using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Appointments;

public class AppointmentDtoValidator : AbstractValidator<AppointmentResponse>
{
    public AppointmentDtoValidator(bool showNurses, bool showRooms)
    {
        RuleFor(x => x.DoctorId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Doctor));

        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Patient));

        RuleFor(x => x.AppointmentEndTime)
            .GreaterThanOrEqualTo(x => x.AppointmentStartTime)
            .WithMessage(Resource.EndTimeCannotBeEarlierThanStartTime);

        When(_ => showNurses, () =>
        {
            RuleFor(x => x.NurseId)
                .NotNull()
                .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Nurse));
        });

        When(_ => showRooms, () =>
        {
            RuleFor(x => x.RoomId)
                .NotNull()
                .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Room));
        });
    }
}
