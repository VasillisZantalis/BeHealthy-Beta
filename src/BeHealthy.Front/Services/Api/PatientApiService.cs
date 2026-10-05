using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.User;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Front.Services.Api;

public class PatientApiService : ApiClientBase, IPatientService
{
    public PatientApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<IEnumerable<PatientResponse>> GetAllPatientsAsync(PatientQueryParameters? parameters = null)
        => await GetListAsync<PatientResponse>($"patients{ToQueryString(parameters)}");

    public async Task<PatientResponse?> GetPatientByIdAsync(int id)
        => await GetAsync<PatientResponse>($"patients/{id}");

    public async Task<IEnumerable<PatientSimpleResponse>> GetAllPatientsSimpleAsync()
        => await GetListAsync<PatientSimpleResponse>("patients/simple");

    public async Task<IEnumerable<DoctorResponse>> GetMyDoctorsAsync()
        => await GetListAsync<DoctorResponse>("me/doctors");

    public async Task<ServiceResponse> AddPatientAsync(PatientCreateRequest patient)
        => await PostForResponseAsync("patients", patient);

    public async Task<int> GetPatientCountAsync()
        => await GetAsync<int>("patients/count");

    public async Task<ServiceResponse> UpdatePatientAsync(PatientUpdateRequest patient)
        => await PutForResponseAsync($"patients/{patient.Id}", patient);

    public async Task DeletePatientAsync(int id)
        => await DeleteAsync($"patients/{id}");
}
