using BeHealthy.Shared.Dtos.Auth;

namespace BeHealthy.Validation.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.Username));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Required(Resource.Password));
    }
}
