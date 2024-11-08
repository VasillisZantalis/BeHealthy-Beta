using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Patients
                    .Include(d => d.User)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Patient!.UserId == userId)
                .ToListAsync();
    }

    public async Task DeletePatientAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();
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
}
