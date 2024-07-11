using BeHealthy.Shared.Models.Dtos.Appointment;

namespace BeHealthy.Client.Services.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(string doctorId);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(string patientId);
    Task<AppointmentDto> GetAppointmentByIdAsync(int id);
    Task AddAppointmentAsync(AppointmentForCreationDto appointmentDto);
    Task UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointmentDto);
    Task DeleteAppointmentAsync(int id);
}
