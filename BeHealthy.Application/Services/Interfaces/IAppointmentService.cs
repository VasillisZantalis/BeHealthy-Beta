using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Application.Dtos.Common;
using BeHealthy.Domain;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<ServiceResponse> AddAppointmentAsync(AppointmentForCreationDto appointment);
    Task<ServiceResponse> UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointment);
    Task DeleteAppointmentAsync(int id);
}
