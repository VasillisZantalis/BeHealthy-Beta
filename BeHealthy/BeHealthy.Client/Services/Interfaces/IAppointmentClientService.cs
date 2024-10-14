using BeHealthy.Shared.Models.Dtos.Appointment;

namespace BeHealthy.Client.Services.Interfaces;

public interface IAppointmentClientService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByUserIdAsync(string userId);
    Task<AppointmentDto> GetAppointmentByIdAsync(int id);
    Task AddAppointmentAsync(AppointmentForCreationDto appointmentDto);
    Task UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointmentDto);
    Task DeleteAppointmentAsync(int id);
}
