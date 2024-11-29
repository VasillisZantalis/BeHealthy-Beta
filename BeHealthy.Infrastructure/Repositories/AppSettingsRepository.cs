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

    public async Task<AppSetting?> GetSettingByKeyAsync(string key)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.AppSettings.FirstOrDefaultAsync(w => w.Key == key);
    }
}
