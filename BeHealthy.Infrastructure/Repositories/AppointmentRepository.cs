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

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByNurseIdAsync(int nurseId)
    {
        return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(i => i.Room)
                .Include(i => i.Nurse)
                .Where(a => a.NurseId == nurseId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByUserIdAsync(string userId)
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
                    .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId)
    {
        return await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(i => i.Room)
                    .Include(i => i.Nurse)
                    .Where(a => a.Doctor!.UserId == userId || a.Patient!.UserId == userId || a.Nurse!.UserId == userId)
                    .ToListAsync();
    }
}
