using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Patient;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class PatientClientService : IPatientClientService
{
    private readonly HttpClient _httpClient;

    public PatientClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
    {
        var patients = await _httpClient.GetFromJsonAsync<IEnumerable<PatientDto>>("api/patients");
        return patients ?? new List<PatientDto>();
    }

    public async Task<PatientDto>? GetPatientByIdAsync(int id)
    {
        var patients = await _httpClient.GetFromJsonAsync<PatientDto>($"api/patients/{id}");
        return patients ?? null!;
    }

    public async Task AddPatientAsync(PatientForCreationDto patientDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/patients", patientDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePatientAsync(int id, PatientForUpdateDto patientDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/patients/{id}", patientDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePatientAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/patients/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AppointmentDto>>($"api/patients/{userId}/appointments") ?? new List<AppointmentDto>();
    }
}
