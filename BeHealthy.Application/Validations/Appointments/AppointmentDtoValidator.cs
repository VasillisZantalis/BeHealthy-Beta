using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.Appointments;

public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
{
    public AppointmentDtoValidator(bool showNurses, bool showRooms)
    {
        RuleFor(x => x.DoctorId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Doctor));

        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage(string.Format(Resource.TheFieldIsRequired, Resource.Patient));

        RuleFor(x => x.Duration)
            .InclusiveBetween(1, 1440)
            .WithMessage(string.Format(Resource.MustBeBetween, Resource.Duration, 1, 1440));

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
