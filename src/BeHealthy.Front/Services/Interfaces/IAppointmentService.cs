using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Front.Services.Interfaces;

public interface IAppointmentService
{
    Task<PaginatedResult<AppointmentResponse>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentResponse>> GetMyAppointmentsAsync();
    Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts();
    Task<AppointmentResponse?> GetAppointmentByIdAsync(int id);
    Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateRequest appointment);
    Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateRequest appointment);
    Task DeleteAppointmentAsync(int id);
    Task<List<AppointmentResponse>> GetUpcomingAppointments();
}
