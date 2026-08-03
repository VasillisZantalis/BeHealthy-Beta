using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Frontend.Services.Api;

public class SpecialtyApiService : ApiClientBase, ISpecialtyService
{
    public SpecialtyApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync()
        => await GetListAsync<SpecialtyResponse>("specialties");

    public async Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id)
        => await GetAsync<SpecialtyResponse>($"specialties/{id}");

    public async Task AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto)
        => await PostAsync("specialties", specialtyForCreationDto);

    public async Task UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto)
        => await PutAsync("specialties", specialtyForUpdateDto);

    public async Task DeleteSpecialtyAsync(int id)
        => await DeleteAsync($"specialties/{id}");
}
