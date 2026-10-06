using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Shared.Parameters;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Patients.Include(d => d.User).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Patients.ToListAsync(cancellationToken);
    }

    public async Task<bool> DeletePatientAsync(int id, CancellationToken cancellationToken = default)
    {
        var patient = await _context.Patients
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (patient is null)
        {
            return false;
        }

        if (patient.User != null)
        {
            _context.Users.Remove(patient.User);
        }

        _context.Patients.Remove(patient);
        return true;
    }

    public async Task<bool> HasClinicalHistoryAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.AnyAsync(a => a.PatientId == patientId, cancellationToken)
            || await _context.Visits.AnyAsync(v => v.PatientId == patientId, cancellationToken)
            || await _context.MedicalRecords.AnyAsync(m => m.PatientId == patientId, cancellationToken)
            || await _context.Prescriptions.AnyAsync(p => p.PatientId == patientId, cancellationToken)
            || await _context.Allergies.AnyAsync(a => a.PatientId == patientId, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(w => w.DepartmentId == departmentId)
            .Include(i => i.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(i => i.User)
            .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);
    }
}
