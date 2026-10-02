using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAppSettingsService
{
    Task<IEnumerable<AppSetting>> GetAppSettingsAsync(CancellationToken cancellationToken = default);
    Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys, CancellationToken cancellationToken = default);
    Task<AppSetting?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task UpdateSettingAsync(AppSetting setting, CancellationToken cancellationToken = default);
}
