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

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
                    .Include(d => d.User)
                    .Include(d => d.Specialty)
                    .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsSimpleAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteDoctorAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (doctor is null)
        {
            return false;
        }

        if (doctor.User != null)
        {
            _context.Users.Remove(doctor.User);
        }

        _context.Doctors.Remove(doctor);
        return true;
    }

    public async Task<bool> HasClinicalHistoryAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.AnyAsync(a => a.DoctorId == doctorId, cancellationToken)
            || await _context.Visits.AnyAsync(v => v.DoctorId == doctorId, cancellationToken)
            || await _context.Prescriptions.AnyAsync(p => p.DoctorId == doctorId, cancellationToken);
    }

    public async Task<Doctor?> GetDoctorByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .Include(i => i.User)
            .FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);
    }

    public async Task<bool> IsDoctorHeadOfDepartmentAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Departments.AnyAsync(w => w.HeadOfDepartmentId == doctorId, cancellationToken);
    }
}
