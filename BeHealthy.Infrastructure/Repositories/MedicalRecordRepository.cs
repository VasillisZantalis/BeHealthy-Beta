using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientIdAsync(int patientId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var records = await context.MedicalRecords
                                   .Where(mr => mr.PatientId == patientId)
                                   .ToListAsync();
        return records;
    }
}
