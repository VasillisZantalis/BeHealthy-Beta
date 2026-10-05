using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Front.Services.Interfaces;

public interface IAppSettingsService
{
    Task<IEnumerable<AppSettingResponse>> GetAppSettingsAsync();
    Task<List<AppSettingResponse>> GetMassAppSettingsAsync(List<string> keys);
    Task<AppSettingResponse?> GetSettingByKeyAsync(string key);
    Task UpdateSettingAsync(AppSettingUpdateRequest setting);
}
