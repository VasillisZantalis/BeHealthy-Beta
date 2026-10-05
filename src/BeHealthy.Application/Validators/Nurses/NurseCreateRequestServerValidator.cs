using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Nurses;

public class NurseCreateRequestServerValidator : AbstractValidator<NurseCreateRequest>
{
    public NurseCreateRequestServerValidator(IUserService userService, IDepartmentRepository departments)
    {
        RuleFor(x => x.Email).MustBeUnusedEmail(userService);
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
    }
}
