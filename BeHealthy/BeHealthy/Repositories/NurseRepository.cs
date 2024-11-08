using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class NurseRepository : GenericRepository<Nurse>, INurseRepository
{
    public NurseRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Nurse>> GetAllNursesAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Nurses
                    .Include(d => d.User)
                    .ToListAsync();
    }

    public async Task DeleteNurseAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();
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
}
