using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class AppSettingsRepository : GenericRepository<AppSetting>, IAppSettingsRepository
{
    public AppSettingsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<AppSetting>> GetMassAppSettingsAsync(List<string> keys, CancellationToken cancellationToken = default)
    {
        return await _context.AppSettings
            .AsNoTracking()
            .Where(w => keys.Contains(w.Key))
            .ToListAsync(cancellationToken);
    }

    public async Task<AppSetting?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.AppSettings.FirstOrDefaultAsync(w => w.Key == key, cancellationToken);
    }
}
