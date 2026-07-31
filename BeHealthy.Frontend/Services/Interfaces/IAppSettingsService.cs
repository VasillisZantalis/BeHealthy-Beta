using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IAppSettingsService
{
    Task<IEnumerable<AppSettingDto>> GetAppSettingsAsync();
    Task<List<AppSettingDto>> GetMassAppSettingsAsync(List<string> keys);
    Task<AppSettingDto?> GetSettingByKeyAsync(string key);
    Task UpdateSettingAsync(AppSettingUpdateDto setting);
}
