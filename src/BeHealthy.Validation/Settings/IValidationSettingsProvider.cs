namespace BeHealthy.Validation.Settings;

/// <summary>
/// Reads the admin-configurable app settings that switch validation rules on or off, for example
/// "a doctor must have a specialty". Validators get it through DI instead of a constructor flag,
/// so the same validator class enforces the same rules wherever it runs:
/// the API implements it against the database, the Blazor front end against the API.
/// </summary>
public interface IValidationSettingsProvider
{
    /// <returns><c>true</c> when the checkbox setting exists and is switched on.</returns>
    Task<bool> IsEnabledAsync(string settingKey, CancellationToken cancellationToken = default);
}
