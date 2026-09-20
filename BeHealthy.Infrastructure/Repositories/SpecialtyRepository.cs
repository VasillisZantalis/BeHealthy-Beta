using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

internal class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
{
    public SpecialtyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Specialty>> GetAllSpecialtiesAsync()
    {
        return await _context.Specialties.ToListAsync();
    }
}
