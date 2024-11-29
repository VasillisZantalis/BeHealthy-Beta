using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;

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

    public async Task<AppSetting?> GetSettingByKeyAsync(string key)
    {
        return await _unitOfWork.AppSettingsRepository.GetSettingByKeyAsync(key);
    }

    public async Task UpdateSettingAsync(AppSetting setting)
    {
        await _unitOfWork.AppSettingsRepository.UpdateAsync(setting);
    }
}
