using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

internal class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
{
    public SpecialtyRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<List<Specialty>> GetAllSpecialtiesAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Specialties.ToListAsync();
    }
}
