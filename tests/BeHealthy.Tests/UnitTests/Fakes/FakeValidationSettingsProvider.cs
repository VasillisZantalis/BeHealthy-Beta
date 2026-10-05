using BeHealthy.Validation.Settings;

namespace BeHealthy.Tests.UnitTests.Fakes;

/// <summary>Answers <see cref="IsEnabledAsync"/> with <c>true</c> only for the keys it was created with.</summary>
public class FakeValidationSettingsProvider : IValidationSettingsProvider
{
    private readonly HashSet<string> _enabledKeys;

    public FakeValidationSettingsProvider(params string[] enabledKeys)
    {
        _enabledKeys = new HashSet<string>(enabledKeys);
    }

    public Task<bool> IsEnabledAsync(string settingKey, CancellationToken cancellationToken = default)
        => Task.FromResult(_enabledKeys.Contains(settingKey));
}
