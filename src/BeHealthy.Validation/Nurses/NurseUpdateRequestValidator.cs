using BeHealthy.Shared.Dtos.Nurse;

namespace BeHealthy.Validation.Nurses;

public class NurseUpdateRequestValidator : AbstractValidator<NurseUpdateRequest>
{
    public NurseUpdateRequestValidator()
    {
        RuleFor(x => x.Id).EntityId();
        RuleFor(x => x.FirstName).PersonName(Resource.FirstName);
        RuleFor(x => x.LastName).PersonName(Resource.LastName);
        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Image).Image();
        RuleFor(x => x.DepartmentId).OptionalReference(Resource.Department);
    }
}
