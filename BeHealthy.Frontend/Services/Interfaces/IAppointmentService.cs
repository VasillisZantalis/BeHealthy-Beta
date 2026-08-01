using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IAppointmentService
{
    Task<PaginatedResult<AppointmentDto>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateDto appointment);
    Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateDto appointment);
    Task DeleteAppointmentAsync(int id);
}
