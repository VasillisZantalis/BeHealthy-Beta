using BeHealthy.Shared.Dtos.Allergy;

namespace BeHealthy.Validation.Allergies;

public class AllergyCreateRequestValidator : AbstractValidator<AllergyCreateRequest>
{
    public AllergyCreateRequestValidator()
    {
        RuleFor(x => x.AllergyName).RequiredText(Resource.Allergy, FieldLengths.AllergyName);
        RuleFor(x => x.Allergen).OptionalText(Resource.Allergen, FieldLengths.Allergen);
        RuleFor(x => x.Severity).DefinedEnum(Resource.Severity);
        RuleFor(x => x.Notes).OptionalText(Resource.Notes, FieldLengths.AllergyNotes);
        RuleFor(x => x.PatientId).RequiredReference(Resource.Patient);
    }
}
