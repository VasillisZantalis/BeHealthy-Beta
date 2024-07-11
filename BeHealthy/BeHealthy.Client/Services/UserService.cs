using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Entities;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BeHealthy.Client.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
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
}
