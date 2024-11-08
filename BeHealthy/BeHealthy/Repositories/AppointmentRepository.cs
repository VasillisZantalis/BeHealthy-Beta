using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{

    public AppointmentRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsByUserIdAsync(string userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.Doctor!.UserId == userId || a.Doctor!.UserId == userId)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.Doctor!.UserId == userId || a.Patient!.UserId == userId)
                    .ToListAsync();
    }
}
