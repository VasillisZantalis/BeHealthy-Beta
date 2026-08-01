using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Api;

public class AppointmentApiService : ApiClientBase, IAppointmentService
{
    public AppointmentApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<PaginatedResult<AppointmentDto>> GetAllAppointmentsAsync(AppointmentQueryParameters? parameters = null)
        => await GetAsync<PaginatedResult<AppointmentDto>>($"appointments{ToQueryString(parameters)}") ?? new();

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
        => await GetListAsync<AppointmentDto>($"appointments/doctor/{doctorId}");

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId)
        => await GetListAsync<AppointmentDto>($"appointments/patient/{patientId}");

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId)
        => await GetListAsync<AppointmentDto>($"appointments/user/{userId}");

    public async Task<Dictionary<AppointmentReason, int>> GetAppointmentReasonCounts()
        => await GetAsync<Dictionary<AppointmentReason, int>>("appointments/reason-counts") ?? new();

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        => await GetAsync<AppointmentDto>($"appointments/{id}");

    public async Task<ServiceResponse> AddAppointmentAsync(AppointmentCreateDto appointment)
        => await PostForResponseAsync("appointments", appointment);

    public async Task<ServiceResponse> UpdateAppointmentAsync(AppointmentUpdateDto appointment)
        => await PutForResponseAsync("appointments", appointment);

    public async Task DeleteAppointmentAsync(int id)
        => await DeleteAsync($"appointments/{id}");
}
