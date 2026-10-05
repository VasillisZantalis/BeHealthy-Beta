using BeHealthy.Front.Helpers;
using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Validation.Settings;

namespace BeHealthy.Front.Services.Validation;

/// <summary>
/// Front-end source of the settings that switch shared validation rules on: the settings API.
/// The API re-checks every request against the database, so a setting an admin changed after this
/// page loaded is still enforced, and the form shows the API's message.
/// </summary>
public sealed class ValidationSettingsProvider : IValidationSettingsProvider
{
    private readonly IAppSettingsService _appSettingsService;

    public ValidationSettingsProvider(IAppSettingsService appSettingsService)
    {
        _appSettingsService = appSettingsService;
    }

    public async Task<bool> IsEnabledAsync(string settingKey, CancellationToken cancellationToken = default)
    {
        var setting = await _appSettingsService.GetSettingByKeyAsync(settingKey);
        return setting?.GetBooleanValue() ?? false;
    }
}
