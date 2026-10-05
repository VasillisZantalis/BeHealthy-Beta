using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Doctors;

public class DoctorUpdateRequestServerValidator : AbstractValidator<DoctorUpdateRequest>
{
    public DoctorUpdateRequestServerValidator(IDepartmentRepository departments, ISpecialtyRepository specialties)
    {
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
        RuleFor(x => x.SpecialtyId).MustExist(specialties, Resource.Specialty);
    }
}
