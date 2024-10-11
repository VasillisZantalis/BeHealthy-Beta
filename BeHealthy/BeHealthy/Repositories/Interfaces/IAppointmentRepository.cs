using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId);
}
