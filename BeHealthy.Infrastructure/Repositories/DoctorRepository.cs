using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Doctors
                    .Include(d => d.User)
                    .ToListAsync();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
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
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Where(a => a.Doctor!.UserId == userId)
                .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorByUserIdAsync(string userId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Doctors
            .Include(i => i.User)
            .Where(w => w.UserId == userId)
            .Select(s => new Doctor
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Specialty = s.Specialty,
                UserId = userId,
                Image = s.Image,
                User = s.User
            })
            .FirstOrDefaultAsync();
    }
}
