using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Patients;

public class PatientCreateRequestServerValidator : AbstractValidator<PatientCreateRequest>
{
    public PatientCreateRequestServerValidator(IUserService userService, IDepartmentRepository departments)
    {
        RuleFor(x => x.Email).MustBeUnusedEmail(userService);
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
    }
}
