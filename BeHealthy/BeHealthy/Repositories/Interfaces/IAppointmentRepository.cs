using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId);
}
