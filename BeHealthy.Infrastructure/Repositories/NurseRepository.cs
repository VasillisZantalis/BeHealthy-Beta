using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class NurseRepository : GenericRepository<Nurse>, INurseRepository
{
    public NurseRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Nurse>> GetAllNursesAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Nurses
                    .Include(d => d.User)
                    .ToListAsync();
    }

    public async Task DeleteNurseAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var nurse = await context.Nurses
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

        if (nurse != null)
        {
            if (nurse.User != null)
            {
                context.Users.Remove(nurse.User);
            }

            context.Nurses.Remove(nurse);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Nurse?> GetNurseByUserIdAsync(string userId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        return context.Nurses
            .Include(n => n.User)
            .FirstOrDefault(n => n.UserId == userId);
    }
}
