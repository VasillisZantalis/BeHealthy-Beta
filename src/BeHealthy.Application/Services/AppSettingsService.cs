using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Services;

public class AppSettingsService : IAppSettingsService
{
    private readonly IAppSettingsRepository _appSettingsRepository;

    public AppSettingsService(IAppSettingsRepository appSettingsRepository)
    {
        _appSettingsRepository = appSettingsRepository;
    }

    public async Task<IEnumerable<AppSetting>> GetAppSettingsAsync(CancellationToken cancellationToken = default)
    {
        return await _appSettingsRepository.GetAllAsync(cancellationToken);
    }

    public async Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys, CancellationToken cancellationToken = default)
    {
        return await _appSettingsRepository.GetMassAppSettingsAsync(keys, cancellationToken);
    }

    public async Task<AppSetting?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _appSettingsRepository.GetSettingByKeyAsync(key, cancellationToken);
    }

    public async Task UpdateSettingAsync(AppSetting setting, CancellationToken cancellationToken = default)
    {
        await _appSettingsRepository.UpdateAsync(setting);
        await _appSettingsRepository.SaveChangesAsync(cancellationToken);
    }
}
