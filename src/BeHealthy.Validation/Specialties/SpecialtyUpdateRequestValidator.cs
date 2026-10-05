using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Validation.Specialties;

public class SpecialtyUpdateRequestValidator : AbstractValidator<SpecialtyUpdateRequest>
{
    public SpecialtyUpdateRequestValidator()
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.Name).RequiredText(Resource.Name, FieldLengths.SpecialtyName);
    }
}
