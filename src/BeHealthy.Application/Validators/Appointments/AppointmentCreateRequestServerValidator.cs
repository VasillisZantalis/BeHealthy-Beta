using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Appointments;

/// <summary>
/// Overlapping bookings are not checked here: that depends on the state of the database at the
/// moment of writing, so AppointmentService checks it as part of the write.
/// </summary>
public class AppointmentCreateRequestServerValidator : AbstractValidator<AppointmentCreateRequest>
{
    public AppointmentCreateRequestServerValidator(IPatientRepository patients, IDoctorRepository doctors, IRoomRepository rooms, INurseRepository nurses)
    {
        RuleFor(x => x.PatientId).MustExist(patients, Resource.Patient);
        RuleFor(x => x.DoctorId).MustExist(doctors, Resource.Doctor);
        RuleFor(x => x.RoomId).MustExist(rooms, Resource.Room);
        RuleFor(x => x.NurseId).MustExist(nurses, Resource.Nurse);
    }
}
