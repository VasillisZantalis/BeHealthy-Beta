using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validations.User;

public class ProfileDtoValidator : AbstractValidator<ProfileDto>
{
    public ProfileDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(string.Format(Resource.PropertyRequired, Resource.FirstName))
            .MaximumLength(50).WithMessage(string.Format(Resource.PropertyMaxCharacters, Resource.FirstName, 50));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(string.Format(Resource.PropertyRequired, Resource.LastName))
            .MaximumLength(50).WithMessage(string.Format(Resource.PropertyMaxCharacters, Resource.LastName, 50));
    }
}