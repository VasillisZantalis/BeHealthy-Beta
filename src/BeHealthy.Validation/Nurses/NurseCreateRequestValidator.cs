using BeHealthy.Shared.Dtos.Nurse;

namespace BeHealthy.Validation.Nurses;

public class NurseCreateRequestValidator : AbstractValidator<NurseCreateRequest>
{
    public NurseCreateRequestValidator()
    {
        RuleFor(x => x.FirstName).PersonName(Resource.FirstName);
        RuleFor(x => x.LastName).PersonName(Resource.LastName);
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();

        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.ConfirmPassword))
            .Equal(x => x.Password).WithMessage(Resource.PasswordsDoNotMatch);

        RuleFor(x => x.PhoneNumber).PhoneNumber();
        RuleFor(x => x.Image).Image();
        RuleFor(x => x.DepartmentId).OptionalReference(Resource.Department);
    }
}
