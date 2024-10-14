using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Nurse;
using System.Net.Http;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services.Interfaces;

public class NurseClientService : INurseClientService
{
    private readonly HttpClient _httpClient;

    public NurseClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddNurseAsync(NurseForCreationDto nurseDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/nurses", nurseDto);
        response.EnsureSuccessStatusCode();
    }
    public async Task UpdateNurseAsync(int id, NurseForUpdateDto nurseDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/nurses/{id}", nurseDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteNurseAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/nurses/{id}");
        response.EnsureSuccessStatusCode();
    }

}
