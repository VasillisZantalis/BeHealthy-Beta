using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync(string? firstName = null, string? lastName = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Patients.Include(d => d.User).AsQueryable();

        if (!string.IsNullOrEmpty(firstName))
        {
            query = query.Where(x => x.FirstName.Contains(firstName));
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            query = query.Where(x => x.LastName.Contains(lastName));
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
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
}
