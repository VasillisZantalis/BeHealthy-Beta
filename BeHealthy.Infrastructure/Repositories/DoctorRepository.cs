using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        return await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.Specialty)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsSimpleAsync()
    {
        return await _context.Doctors.ToListAsync();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

        if (doctor != null)
        {
            if (doctor.User != null)
            {
                _context.Users.Remove(doctor.User);
            }

            _context.Doctors.Remove(doctor);
        }
    }

    public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.Doctor != null && a.Doctor.UserId == userId)
                .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorByUserIdAsync(string userId)
    {
        return await _context.Doctors
            .Include(i => i.User)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task<bool> IsDoctorHeadOfDepartmentAsync(int doctorId)
    {
        return await _context.Departments.AnyAsync(w => w.HeadOfDepartmentId == doctorId);
    }
}
