using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IAppSettingsRepository : IGenericRepository<AppSetting>
{
    Task<AppSetting?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys, CancellationToken cancellationToken = default);
}
