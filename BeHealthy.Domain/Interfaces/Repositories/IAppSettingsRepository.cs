using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IAppSettingsRepository : IGenericRepository<AppSetting>
{
    Task<AppSetting?> GetSettingByKeyAsync(string key);
}
