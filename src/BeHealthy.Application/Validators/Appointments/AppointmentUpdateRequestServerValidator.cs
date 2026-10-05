using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Appointments;

public class AppointmentUpdateRequestServerValidator : AbstractValidator<AppointmentUpdateRequest>
{
    public AppointmentUpdateRequestServerValidator(IPatientRepository patients, IDoctorRepository doctors, IRoomRepository rooms, INurseRepository nurses)
    {
        RuleFor(x => x.PatientId).MustExist(patients, Resource.Patient);
        RuleFor(x => x.DoctorId).MustExist(doctors, Resource.Doctor);
        RuleFor(x => x.RoomId).MustExist(rooms, Resource.Room);
        RuleFor(x => x.NurseId).MustExist(nurses, Resource.Nurse);
    }
}
