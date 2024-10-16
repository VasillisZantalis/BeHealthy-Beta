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

    public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Doctor!.UserId == userId)
                .ToListAsync();
        };
    }

    //public async Task<Doctor> GetDoctorByUserIdAsync(string id)
    //{
    //    return await _context.Doctors.FirstOrDefaultAsync(x => x.UserId == id);
    //}
}
