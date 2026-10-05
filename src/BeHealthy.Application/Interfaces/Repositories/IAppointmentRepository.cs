using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByNurseIdAsync(int nurseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> GetAllAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(string userId, CancellationToken cancellationToken = default);
}
