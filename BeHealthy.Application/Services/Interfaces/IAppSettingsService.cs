using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAppSettingsService
{
    Task<IEnumerable<AppSetting>> GetAppSettingsAsync();
    Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys);
    Task<AppSetting?> GetSettingByKeyAsync(string key);
    Task UpdateSettingAsync(AppSetting setting);
}
