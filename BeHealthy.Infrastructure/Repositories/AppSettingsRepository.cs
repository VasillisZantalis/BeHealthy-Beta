using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class AppSettingsRepository : GenericRepository<AppSetting>, IAppSettingsRepository
{
    public AppSettingsRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
    
        return await context.AppSettings
            .AsNoTracking()
            .Where(w => keys.Contains(w.Key))
            .ToListAsync();
    }

    public async Task<AppSetting?> GetSettingByKeyAsync(string key)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.AppSettings.FirstOrDefaultAsync(w => w.Key == key);
    }
}
