using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Frontend.Services.Api;

public class PatientApiService : ApiClientBase, IPatientService
{
    public PatientApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(PatientQueryParameters? parameters = null)
        => await GetListAsync<PatientDto>($"patients{ToQueryString(parameters)}");

    public async Task<PatientDto?> GetPatientByIdAsync(int id)
        => await GetAsync<PatientDto>($"patients/{id}");

    public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId)
        => await GetListAsync<AppointmentDto>($"patients/{userId}/appointments");

    public async Task<IEnumerable<PatientSimpleDto>> GetAllPatientsSimpleAsync()
        => await GetListAsync<PatientSimpleDto>("patients/simple");

    public async Task<ProfileDto?> GetPatientProfileByUserIdAsync(string userId)
        => await GetAsync<ProfileDto>($"patients/{userId}/profile");

    public async Task<IEnumerable<DoctorDto>> GetMyDoctorsAsync(string userId)
        => await GetListAsync<DoctorDto>($"patients/{userId}/doctors");

    public async Task<ServiceResponse> AddPatientAsync(PatientCreateDto patient)
        => await PostForResponseAsync("patients", patient);

    public async Task<int> GetPatientCountAsync()
        => await GetAsync<int>("patients/count");

    public async Task<ServiceResponse> UpdatePatientAsync(PatientUpdateDto patient)
        => await PutForResponseAsync("patients", patient);

    public async Task DeletePatientAsync(int id)
        => await DeleteAsync($"patients/{id}");
}
