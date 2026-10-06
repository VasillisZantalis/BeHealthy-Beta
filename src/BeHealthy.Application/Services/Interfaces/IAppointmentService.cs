using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<PaginatedResult<AppointmentResponse>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts(CancellationToken cancellationToken = default);
    Task<AppointmentResponse?> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateRequest appointment, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateRequest appointment, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteAppointmentAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentResponse>> GetUpcomingAppointmentsAsync(CancellationToken cancellationToken = default);
}
