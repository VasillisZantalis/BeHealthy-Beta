using BeHealthy.Shared.Common;
using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Validation.Settings;

namespace BeHealthy.Validation.Departments;

public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest>
{
    public DepartmentCreateRequestValidator(IValidationSettingsProvider settings)
    {
        RuleFor(x => x.Name).RequiredText(Resource.Name, FieldLengths.DepartmentName);
        RuleFor(x => x.Location).OptionalText(Resource.Location, FieldLengths.DepartmentLocation);
        RuleFor(x => x.HeadOfDepartmentId).OptionalReference(Resource.HeadΟfDepartment);

        WhenAsync((_, cancellationToken) => settings.IsEnabledAsync(AppSettingKeys.DepartmentRequiresSupervisor, cancellationToken), () =>
        {
            RuleFor(x => x.HeadOfDepartmentId).NotNull().WithMessage(ValidationMessages.Required(Resource.HeadΟfDepartment));
        });
    }
}
