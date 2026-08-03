using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IAppointmentService
{
    Task<PaginatedResult<AppointmentResponse>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByUserIdAsync(string userId);
    Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts();
    Task<AppointmentResponse?> GetAppointmentByIdAsync(int id);
    Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateRequest appointment);
    Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateRequest appointment);
    Task DeleteAppointmentAsync(int id);
}
