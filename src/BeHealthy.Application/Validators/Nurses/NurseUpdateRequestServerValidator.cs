using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Nurses;

public class NurseUpdateRequestServerValidator : AbstractValidator<NurseUpdateRequest>
{
    public NurseUpdateRequestServerValidator(IDepartmentRepository departments)
    {
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
    }
}
