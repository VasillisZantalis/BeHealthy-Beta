using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{

    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByUserIdAsync(string userId)
    {
        return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Doctor!.UserId == userId || a.Doctor!.UserId == userId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId)
    {
        return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Doctor!.UserId == userId || a.Patient!.UserId == userId)
                .ToListAsync();
    }
}
