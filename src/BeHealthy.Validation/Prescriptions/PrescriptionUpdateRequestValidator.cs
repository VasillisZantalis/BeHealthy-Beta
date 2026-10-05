using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Validation.Prescriptions;

public class PrescriptionUpdateRequestValidator : AbstractValidator<PrescriptionUpdateRequest>
{
    public PrescriptionUpdateRequestValidator()
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.Medication).RequiredText(Resource.Medication, FieldLengths.Medication);
        RuleFor(x => x.Dosage).RequiredText(Resource.Dosage, FieldLengths.Dosage);
    }
}
