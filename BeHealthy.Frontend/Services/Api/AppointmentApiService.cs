using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Api;

public class AppointmentApiService : ApiClientBase, IAppointmentService
{
    public AppointmentApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<PaginatedResult<AppointmentResponse>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null)
        => await GetAsync<PaginatedResult<AppointmentResponse>>($"appointments{ToQueryString(parameters)}") ?? new();

    public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
        => await GetListAsync<AppointmentResponse>($"appointments/doctor/{doctorId}");

    public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByPatientIdAsync(int patientId)
        => await GetListAsync<AppointmentResponse>($"appointments/patient/{patientId}");

    public async Task<IEnumerable<AppointmentResponse>> GetAllAppointmentsByUserIdAsync(string userId)
        => await GetListAsync<AppointmentResponse>($"appointments/user/{userId}");

    public async Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts()
        => await GetAsync<Dictionary<AppointmentReason, int>>("appointments/reason-counts") ?? new();

    public async Task<AppointmentResponse?> GetAppointmentByIdAsync(int id)
        => await GetAsync<AppointmentResponse>($"appointments/{id}");

    public async Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateRequest appointment)
        => await PostForResponseAsync("appointments", appointment);

    public async Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateRequest appointment)
        => await PutForResponseAsync("appointments", appointment);

    public async Task DeleteAppointmentAsync(int id)
        => await DeleteAsync($"appointments/{id}");
}
