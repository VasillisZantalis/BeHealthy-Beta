using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Departments;

public class DepartmentCreateRequestServerValidator : AbstractValidator<DepartmentCreateRequest>
{
    public DepartmentCreateRequestServerValidator(IDoctorRepository doctors)
    {
        RuleFor(x => x.HeadOfDepartmentId).MustExist(doctors, Resource.HeadΟfDepartment);
    }
}
