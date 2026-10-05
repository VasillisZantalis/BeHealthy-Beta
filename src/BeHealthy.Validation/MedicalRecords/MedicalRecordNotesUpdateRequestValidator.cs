using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Validation.MedicalRecords;

public class MedicalRecordNotesUpdateRequestValidator : AbstractValidator<MedicalRecordNotesUpdateRequest>
{
    public MedicalRecordNotesUpdateRequestValidator()
    {
        RuleFor(x => x.Notes).OptionalText(Resource.Notes, FieldLengths.MedicalRecordNotes);
    }
}
