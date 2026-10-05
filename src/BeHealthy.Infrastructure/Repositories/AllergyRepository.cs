using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class AllergyRepository : GenericRepository<Allergy>, IAllergyRepository
{
    public AllergyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Allergy>> GetAllergiesByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Allergies
            .Where(a => a.PatientId == patientId)
            .ToListAsync(cancellationToken);
    }
}
