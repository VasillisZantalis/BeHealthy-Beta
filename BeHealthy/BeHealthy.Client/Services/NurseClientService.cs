using BeHealthy.Shared.Interfaces;
using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Nurse;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class NurseClientService : INurseService
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

    public async Task<IEnumerable<NurseDto>> GetAllNursesAsync()
    {
        var nurses = await _httpClient.GetFromJsonAsync<IEnumerable<NurseDto>>("api/nurses");
        return nurses ?? new List<NurseDto>();
    }

    public Task<NurseDto> GetNurseByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNurseAsync(NurseForUpdateDto nurse)
    {
        throw new NotImplementedException();
    }
}
