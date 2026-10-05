using BeHealthy.Shared.Locales;
using FluentValidation;

namespace BeHealthy.Application.Validators.Settings;

/// <summary>Checks that the new value can be read back as the setting's type.</summary>
public class AppSettingUpdateRequestServerValidator : AbstractValidator<AppSettingUpdateRequest>
{
    public AppSettingUpdateRequestServerValidator(IAppSettingsRepository appSettingsRepository)
    {
        RuleFor(x => x.Value).CustomAsync(async (value, context, cancellationToken) =>
        {
            var setting = await appSettingsRepository.GetSettingByKeyAsync(context.InstanceToValidate.Key, cancellationToken);
            if (setting is null)
            {
                // Unknown key: the controller answers 404.
                return;
            }

            var error = setting.Type switch
            {
                SettingType.Checkbox when !bool.TryParse(value, out _) => string.Format(Resource.SettingValueMustBeBoolean, setting.Caption),
                SettingType.SingleSelect when !int.TryParse(value, out _) => string.Format(Resource.SettingValueMustBeNumber, setting.Caption),
                _ => null
            };

            if (error is not null)
            {
                context.AddFailure(error);
            }
        });
    }
}
