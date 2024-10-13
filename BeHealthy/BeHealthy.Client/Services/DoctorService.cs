using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using BeHealthy.Shared.Models.Dtos.Doctor;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class DoctorService : IDoctorService
{
    private readonly HttpClient _httpClient;

    public DoctorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _httpClient.GetFromJsonAsync<IEnumerable<DoctorDto>>("api/doctors");
        return doctors ?? new List<DoctorDto>();
    }

    public async Task<DoctorDto>? GetDoctorByIdAsync(int id)
    {
        var doctors = await _httpClient.GetFromJsonAsync<DoctorDto>($"api/doctors/{id}");
        return doctors ?? null!;
    }

    public async Task AddDoctorAsync(DoctorForCreationDto doctorDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/doctors", doctorDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDoctorAsync(int id, DoctorForUpdateDto doctorDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/doctors/{id}", doctorDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDoctorAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/doctors/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AppointmentDto>>($"api/doctors/{userId}/appointments") ?? new List<AppointmentDto>();
    }
}
