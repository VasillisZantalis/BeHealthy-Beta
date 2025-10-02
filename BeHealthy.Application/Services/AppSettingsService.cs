using BeHealthy.Application.Interfaces;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Services;

public class AppSettingsService : IAppSettingsService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppSettingsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AppSetting>> GetAppSettingsAsync()
    {
        return await _unitOfWork.AppSettingsRepository.GetAllAsync();
    }

    public async Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys)
    {
        return await _unitOfWork.AppSettingsRepository.GetMassAppSettingsAsync(keys);
    }

    public async Task<AppSetting?> GetSettingByKeyAsync(string key)
    {
        return await _unitOfWork.AppSettingsRepository.GetSettingByKeyAsync(key);
    }

    public async Task UpdateSettingAsync(AppSetting setting)
    {
        await _unitOfWork.AppSettingsRepository.UpdateAsync(setting);
    }
}
