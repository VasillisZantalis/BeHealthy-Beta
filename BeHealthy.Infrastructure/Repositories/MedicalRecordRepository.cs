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

    public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientIdAsync(int patientId)
    {
        var records = await _context.MedicalRecords
                                   .Where(mr => mr.PatientId == patientId)
                                   .ToListAsync();
        return records;
    }

    public async Task UpdateMedicalRecordNotesAsync(int id, string notes)
    {
        await _context.MedicalRecords
            .Where(context => context.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(mr => mr.Notes, notes));
    }
}
