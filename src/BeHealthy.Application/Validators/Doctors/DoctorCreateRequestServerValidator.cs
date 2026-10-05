using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Doctors;

public class DoctorCreateRequestServerValidator : AbstractValidator<DoctorCreateRequest>
{
    public DoctorCreateRequestServerValidator(IUserService userService, IDepartmentRepository departments, ISpecialtyRepository specialties)
    {
        RuleFor(x => x.Email).MustBeUnusedEmail(userService);
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
        RuleFor(x => x.SpecialtyId).MustExist(specialties, Resource.Specialty);
    }
}
