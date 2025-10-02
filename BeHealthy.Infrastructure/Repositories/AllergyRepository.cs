using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class AllergyRepository : GenericRepository<Allergy>, IAllergyRepository
{
    public AllergyRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Allergy>> GetAllergiesByPatientIdAsync(int patientId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Allergies
            .Where(a => a.PatientId == patientId)
            .ToListAsync();
    }
}