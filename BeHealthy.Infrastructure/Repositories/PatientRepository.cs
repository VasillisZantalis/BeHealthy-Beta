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

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        return await _context.Patients.Include(d => d.User).ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.Patient!.UserId == userId)
                .ToListAsync();
    }

    public async Task DeletePatientAsync(int id)
    {
        var patient = await _context.Patients
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

        if (patient != null)
        {
            if (patient.User != null)
            {
                _context.Users.Remove(patient.User);
            }

            _context.Patients.Remove(patient);
        }
    }

    public async Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(w => w.DepartmentId == departmentId)
            .Include(i => i.User)
            .ToListAsync();
    }

    public async Task<Patient?> GetPatientByUserIdAsync(string userId)
    {
        return await _context.Patients
            .Include(i => i.User)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }
}
