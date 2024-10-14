using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BeHealthy.Client.Services;

public class UserClientService : IUserClientService
{
    private readonly HttpClient _httpClient;

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllDoctorsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/users/doctors") ?? new List<ApplicationUser>();
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllPatientsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/users/patients") ?? new List<ApplicationUser>();
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllNursesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/users/nurses") ?? new List<ApplicationUser>();
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllStaffAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/users/staff") ?? new List<ApplicationUser>();
    }

    public async Task DeleteUserAsync(string id)
    {
        await _httpClient.DeleteAsync($"api/users/{id}");
    }
}
