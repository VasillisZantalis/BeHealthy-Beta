using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class NurseRepository : GenericRepository<Nurse>, INurseRepository
{
    public NurseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Nurse>> GetAllNursesAsync()
    {
        return await _context.Nurses
                    .Include(d => d.User)
                    .ToListAsync();
    }

    public async Task DeleteNurseAsync(int id)
    {
        var nurse = await _context.Nurses
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

        if (nurse != null)
        {
            if (nurse.User != null)
            {
                _context.Users.Remove(nurse.User);
            }

            _context.Nurses.Remove(nurse);
        }
    }

    public async Task<Nurse?> GetNurseByUserIdAsync(string userId)
    {
        return await _context.Nurses
            .Include(n => n.User)
            .FirstOrDefaultAsync(n => n.UserId == userId);
    }
}
