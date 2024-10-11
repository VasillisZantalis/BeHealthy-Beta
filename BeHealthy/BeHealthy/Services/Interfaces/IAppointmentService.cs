using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<AppointmentDto> GetAppointmentByIdAsync(int id);
    Task AddAppointmentAsync(AppointmentForCreationDto appointment);
    Task UpdateAppointmentAsync(AppointmentForUpdateDto appointment);
    Task DeleteAppointmentAsync(int id);
}
