using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Validation.Seeding;

public class SeedingOptionsRequestValidator : AbstractValidator<SeedingOptionsRequest>
{
    public const int MaxCount = 1000;

    public SeedingOptionsRequestValidator()
    {
        RuleFor(x => x.DoctorCount).InclusiveBetween(1, MaxCount).When(x => x.SeedDoctors)
            .WithMessage(ValidationMessages.Between(Resource.Doctors, 1, MaxCount));

        RuleFor(x => x.PatientCount).InclusiveBetween(1, MaxCount).When(x => x.SeedPatients)
            .WithMessage(ValidationMessages.Between(Resource.Patients, 1, MaxCount));

        RuleFor(x => x.NurseCount).InclusiveBetween(1, MaxCount).When(x => x.SeedNurses)
            .WithMessage(ValidationMessages.Between(Resource.Nurses, 1, MaxCount));

        RuleFor(x => x.AppointmentCount).InclusiveBetween(1, MaxCount).When(x => x.SeedAppointments)
            .WithMessage(ValidationMessages.Between(Resource.Appointments, 1, MaxCount));
    }
}
