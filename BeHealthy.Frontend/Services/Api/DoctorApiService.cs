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

    public async Task<PaginatedResult<DoctorDto>> GetAllDoctorsAsync(DoctorQueryParameters? parameters = null)
        => await GetAsync<PaginatedResult<DoctorDto>>($"doctors{ToQueryString(parameters)}") ?? new();

    public async Task<IEnumerable<DoctorSimpleDto>> GetAllDoctorsSimpleAsync()
        => await GetListAsync<DoctorSimpleDto>("doctors/simple");

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        => await GetAsync<DoctorDto>($"doctors/{id}");

    public async Task<IEnumerable<PatientDto>> GetMyPatientsAsync(string userId)
        => await GetListAsync<PatientDto>($"doctors/{userId}/patients");

    public async Task<ProfileDto?> GetDoctorProfileByUserIdAsync(string userId)
        => await GetAsync<ProfileDto>($"doctors/{userId}/profile");

    public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId)
        => await GetListAsync<AppointmentDto>($"doctors/{userId}/appointments");

    public async Task<ServiceResponse> AddDoctorAsync(DoctorCreateDto doctor)
        => await PostForResponseAsync("doctors", doctor);

    public async Task<int> GetDoctorCountAsync()
        => await GetAsync<int>("doctors/count");

    public async Task<ServiceResponse> UpdateDoctorAsync(DoctorUpdateDto doctor)
        => await PutForResponseAsync("doctors", doctor);

    public async Task DeleteDoctorAsync(int id)
        => await DeleteAsync($"doctors/{id}");
}
