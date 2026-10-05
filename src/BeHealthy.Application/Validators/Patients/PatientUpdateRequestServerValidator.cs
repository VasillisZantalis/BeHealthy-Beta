using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Patients;

public class PatientUpdateRequestServerValidator : AbstractValidator<PatientUpdateRequest>
{
    public PatientUpdateRequestServerValidator(IDepartmentRepository departments)
    {
        RuleFor(x => x.DepartmentId).MustExist(departments, Resource.Department);
    }
}
