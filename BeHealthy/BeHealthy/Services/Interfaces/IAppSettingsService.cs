using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services.Interfaces;

public interface IAppSettingsService
{
    Task<IEnumerable<AppSetting>> GetAppSettingsAsync();
}
