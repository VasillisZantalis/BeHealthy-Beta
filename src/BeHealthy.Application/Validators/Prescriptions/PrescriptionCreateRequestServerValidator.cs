using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Prescriptions;

public class PrescriptionCreateRequestServerValidator : AbstractValidator<PrescriptionCreateRequest>
{
    public PrescriptionCreateRequestServerValidator(IPatientRepository patients, IDoctorRepository doctors)
    {
        RuleFor(x => x.PatientId).MustExist(patients, Resource.Patient);
        RuleFor(x => x.DoctorId).MustExist(doctors, Resource.Doctor);
    }
}
