using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IAppSettingsService
{
    Task<IEnumerable<AppSettingResponse>> GetAppSettingsAsync();
    Task<List<AppSettingResponse>> GetMassAppSettingsAsync(List<string> keys);
    Task<AppSettingResponse?> GetSettingByKeyAsync(string key);
    Task UpdateSettingAsync(AppSettingUpdateRequest setting);
}
