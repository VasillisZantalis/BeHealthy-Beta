using BeHealthy.Shared.Dtos.Visit;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Visits;

public class VisitCreateRequestServerValidator : AbstractValidator<VisitCreateRequest>
{
    public VisitCreateRequestServerValidator(IPatientRepository patients, IDoctorRepository doctors, IMedicalRecordRepository medicalRecords)
    {
        RuleFor(x => x.PatientId).MustExist(patients, Resource.Patient);
        RuleFor(x => x.DoctorId).MustExist(doctors, Resource.Doctor);
        RuleFor(x => x.MedicalRecordId).MustExist(medicalRecords, Resource.MedicalRecord);
    }
}
