using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Validation.Settings;

/// <summary>
/// Shape only. Whether <see cref="AppSettingUpdateRequest.Value"/> fits the setting's type
/// needs the stored setting, so that check runs on the server.
/// </summary>
public class AppSettingUpdateRequestValidator : AbstractValidator<AppSettingUpdateRequest>
{
    public AppSettingUpdateRequestValidator()
    {
        RuleFor(x => x.Key).RequiredText(Resource.Setting, FieldLengths.AppSettingKey);

        RuleFor(x => x.Value)
            .NotNull().WithMessage(ValidationMessages.Required(Resource.Value))
            .MaximumLength(FieldLengths.AppSettingValue).WithMessage(ValidationMessages.MaxLength(Resource.Value, FieldLengths.AppSettingValue));
    }
}
