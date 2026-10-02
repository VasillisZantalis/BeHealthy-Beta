using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var records = await _context.MedicalRecords
                                   .Where(mr => mr.PatientId == patientId)
                                   .ToListAsync(cancellationToken);
        return records;
    }
}
