using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class VisitRepository : GenericRepository<Visit>, IVisitRepository
{
    public VisitRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<Visit?> GetVisitWithDetailsAsync(int visitId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Visits
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .Include(v => v.MedicalRecord)
            .Include(v => v.Diagnoses)
            .Include(v => v.LabResults)
            .Include(v => v.Treatments)
            .FirstOrDefaultAsync(v => v.Id == visitId);
    }

    public async Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Diagnoses
            .Where(d => d.VisitId == visitId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Treatments
            .Where(t => t.VisitId == visitId)
            .ToListAsync();
    }

    public async Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.LabResults
            .Where(lr => lr.VisitId == visitId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Visit>> GetVisitsByPatientIdAsync(int patientId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Visits
            .Where(v => v.PatientId == patientId)
            .Include(v => v.Patient)
            .Include(v => v.Doctor)
            .ToListAsync();
    }
}