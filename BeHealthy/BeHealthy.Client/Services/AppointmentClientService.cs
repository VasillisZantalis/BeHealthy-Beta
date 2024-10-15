using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Appointment;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class AppointmentClientService : IAppointmentService
{
    private readonly HttpClient _httpClient;

    public AppointmentClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _httpClient.GetFromJsonAsync<IEnumerable<AppointmentDto>>("api/appointments");
        return appointments ?? new List<AppointmentDto>();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByDoctorIdAsync(int doctorId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AppointmentDto>>($"api/appointments/doctor/{doctorId}") ?? new List<AppointmentDto>();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByPatientIdAsync(int patientId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AppointmentDto>>($"api/appointments/patient/{patientId}") ?? new List<AppointmentDto>();
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<AppointmentDto>>($"api/appointments/user/{userId}") ?? new List<AppointmentDto>();
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
    {
        var appointments = await _httpClient.GetFromJsonAsync<AppointmentDto>($"api/appointments/{id}");
        return appointments ?? new AppointmentDto();
    }

    public async Task AddAppointmentAsync(AppointmentForCreationDto appointmentDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/appointments", appointmentDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAppointmentAsync(int id, AppointmentForUpdateDto appointmentDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/appointments/{id}", appointmentDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/appointments/{id}");
        response.EnsureSuccessStatusCode();
    }
}
