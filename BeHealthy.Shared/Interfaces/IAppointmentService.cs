using BeHealthy.Shared.Models.Dtos.Appointment;

namespace BeHealthy.Shared.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<AppointmentDto> GetAppointmentByIdAsync(int id);
    Task AddAppointmentAsync(AppointmentForCreationDto appointment);
    Task UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointment);
    Task DeleteAppointmentAsync(int id);
}
