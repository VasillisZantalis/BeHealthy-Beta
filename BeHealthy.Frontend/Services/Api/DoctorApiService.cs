using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Api;

public class DoctorApiService : ApiClientBase, IDoctorService
{
    public DoctorApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<PaginatedResult<DoctorResponse>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null)
        => await GetAsync<PaginatedResult<DoctorResponse>>($"doctors{ToQueryString(parameters)}") ?? new();

    public async Task<IEnumerable<DoctorSimpleResponse>> GetAllDoctorsSimpleAsync()
        => await GetListAsync<DoctorSimpleResponse>("doctors/simple");

    public async Task<DoctorResponse?> GetDoctorByIdAsync(int id)
        => await GetAsync<DoctorResponse>($"doctors/{id}");

    public async Task<IEnumerable<PatientResponse>> GetMyPatientsAsync(string userId)
        => await GetListAsync<PatientResponse>($"doctors/{userId}/patients");

    public async Task<ProfileResponse?> GetDoctorProfileByUserIdAsync(string userId)
        => await GetAsync<ProfileResponse>($"doctors/{userId}/profile");

    public async Task<IEnumerable<AppointmentResponse>> GetDoctorAppointmentsByUserIdAsync(string userId)
        => await GetListAsync<AppointmentResponse>($"doctors/{userId}/appointments");

    public async Task<ServiceResponse> AddDoctorAsync(DoctorCreateRequest doctor)
        => await PostForResponseAsync("doctors", doctor);

    public async Task<int> GetDoctorCountAsync()
        => await GetAsync<int>("doctors/count");

    public async Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateRequest doctor)
        => await PutForResponseAsync("doctors", doctor);

    public async Task DeleteDoctorAsync(int id)
        => await DeleteAsync($"doctors/{id}");
}
