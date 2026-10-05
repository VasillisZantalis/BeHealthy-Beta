using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Validation.Specialties;

public class SpecialtyCreateRequestValidator : AbstractValidator<SpecialtyCreateRequest>
{
    public SpecialtyCreateRequestValidator()
    {
        RuleFor(x => x.Name).RequiredText(Resource.Name, FieldLengths.SpecialtyName);
    }
}
