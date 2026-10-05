using BeHealthy.Shared.Dtos.MedicalRecord;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.MedicalRecords;

public class MedicalRecordCreateRequestServerValidator : AbstractValidator<MedicalRecordCreateRequest>
{
    public MedicalRecordCreateRequestServerValidator(IPatientRepository patients)
    {
        RuleFor(x => x.PatientId).MustExist(patients, Resource.Patient);
    }
}
