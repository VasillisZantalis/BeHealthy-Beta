using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(int id)
    {
        var prescriptions = await _context.Prescriptions
            .Where(i => i.PatientId == id)
            .Include(i => i.Patient)
            .Include(i => i.Doctor)
            .ToListAsync();

        return prescriptions;
    }
}
