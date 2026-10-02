using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class VisitRepository : GenericRepository<Visit>, IVisitRepository
{
    public VisitRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Visit?> GetVisitWithDetailsAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _context.Visits
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .Include(v => v.MedicalRecord)
            .Include(v => v.Diagnoses)
            .Include(v => v.LabResults)
            .Include(v => v.Treatments)
            .FirstOrDefaultAsync(v => v.Id == visitId, cancellationToken);
    }

    public async Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _context.Diagnoses
            .Where(d => d.VisitId == visitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _context.Treatments
            .Where(t => t.VisitId == visitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _context.LabResults
            .Where(lr => lr.VisitId == visitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Visit>> GetVisitsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Visits
            .Where(v => v.PatientId == patientId)
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .ToListAsync(cancellationToken);
    }
}
