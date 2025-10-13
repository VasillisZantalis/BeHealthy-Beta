using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Shared.Parameters;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync(PatientSearchingParameters patientSearchingParameters)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Patients.Include(d => d.User).AsQueryable();

        if (!string.IsNullOrEmpty(patientSearchingParameters.FirstName))
        {
            query = query.Where(x => x.FirstName.ToLower().Contains(patientSearchingParameters.FirstName.ToLower()));
        }

        if (!string.IsNullOrEmpty(patientSearchingParameters.LastName))
        {
            query = query.Where(x => x.LastName.ToLower().Contains(patientSearchingParameters.LastName.ToLower()));
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Patients.ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.Patient!.UserId == userId)
                .ToListAsync();
    }

    public async Task DeletePatientAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var patient = await context.Patients
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

        if (patient != null)
        {
            if (patient.User != null)
            {
                context.Users.Remove(patient.User);
            }

            context.Patients.Remove(patient);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Patients
            .AsNoTracking()
            .Where(w => w.DepartmentId == departmentId)
            .Include(i => i.User)
            .ToListAsync();
    }
}
