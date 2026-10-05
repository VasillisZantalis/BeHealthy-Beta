using BeHealthy.Application.Helpers;
using BeHealthy.Validation.Settings;

namespace BeHealthy.Application.Validators;

/// <summary>Server-side source of the settings that switch shared validation rules on: the AppSettings table.</summary>
public sealed class ValidationSettingsProvider : IValidationSettingsProvider
{
    private readonly IAppSettingsRepository _appSettingsRepository;

    public ValidationSettingsProvider(IAppSettingsRepository appSettingsRepository)
    {
        _appSettingsRepository = appSettingsRepository;
    }

    public async Task<bool> IsEnabledAsync(string settingKey, CancellationToken cancellationToken = default)
    {
        var setting = await _appSettingsRepository.GetSettingByKeyAsync(settingKey, cancellationToken);
        return setting?.GetBooleanValue() ?? false;
    }
}
