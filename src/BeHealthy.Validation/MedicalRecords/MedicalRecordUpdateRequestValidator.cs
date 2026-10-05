using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Validation.MedicalRecords;

public class MedicalRecordUpdateRequestValidator : AbstractValidator<MedicalRecordUpdateRequest>
{
    public MedicalRecordUpdateRequestValidator()
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.PatientId).RequiredReference(Resource.Patient);
        RuleFor(x => x.Notes).OptionalText(Resource.Notes, FieldLengths.MedicalRecordNotes);
        RuleFor(x => x.RecordDate).NotInTheFuture(Resource.Date);
    }
}
