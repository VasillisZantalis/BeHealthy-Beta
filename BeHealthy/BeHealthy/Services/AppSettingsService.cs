using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

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
}
