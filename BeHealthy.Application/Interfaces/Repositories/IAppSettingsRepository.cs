using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IAppSettingsRepository : IGenericRepository<AppSetting>
{
    Task<AppSetting?> GetSettingByKeyAsync(string key);
    Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys);
}
