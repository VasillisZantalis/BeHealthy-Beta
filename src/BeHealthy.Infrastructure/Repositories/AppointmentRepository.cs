using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{

    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.PatientId == patientId)
                .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(i => i.Room)
                    .Include(i => i.Nurse)
                    .Where(a => (a.Doctor != null && a.Doctor.UserId == userId)
                             || (a.Patient != null && a.Patient.UserId == userId)
                             || (a.Nurse != null && a.Nurse.UserId == userId))
                    .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(i => i.Room)
                    .Include(i => i.Nurse)
                    .Where(a => a.Doctor!.UserId == userId || a.Patient!.UserId == userId || a.Nurse!.UserId == userId)
                    .ToListAsync(cancellationToken);
    }
}
