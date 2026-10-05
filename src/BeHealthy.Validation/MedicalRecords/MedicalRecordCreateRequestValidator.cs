using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Validation.MedicalRecords;

public class MedicalRecordCreateRequestValidator : AbstractValidator<MedicalRecordCreateRequest>
{
    public MedicalRecordCreateRequestValidator()
    {
        RuleFor(x => x.PatientId).RequiredReference(Resource.Patient);
        RuleFor(x => x.Notes).OptionalText(Resource.Notes, FieldLengths.MedicalRecordNotes);
        RuleFor(x => x.RecordDate).NotInTheFuture(Resource.Date);
    }
}
