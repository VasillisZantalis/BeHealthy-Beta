using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var prescriptions = await context.Prescriptions
            .Where(i => i.PatientId == id)
            .Include(i => i.Patient)
            .Include(i => i.Doctor)
            .ToListAsync();

        return prescriptions;
    }
}
