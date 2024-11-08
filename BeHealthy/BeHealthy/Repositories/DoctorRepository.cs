using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Doctors
                    .Include(d => d.User)
                    .ToListAsync();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        var doctor = await context.Doctors
                .Include(d => d.User) 
                .FirstOrDefaultAsync(d => d.Id == id);

        if (doctor != null)
        {
            if (doctor.User != null)
            {
                context.Users.Remove(doctor.User);
            }

            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Doctor!.UserId == userId)
                .ToListAsync();
    }
}
