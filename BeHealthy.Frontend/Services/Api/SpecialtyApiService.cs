using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Frontend.Services.Api;

public class SpecialtyApiService : ApiClientBase, ISpecialtyService
{
    public SpecialtyApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<SpecialtyDto>> GetSpecialtiesAsync()
        => await GetListAsync<SpecialtyDto>("specialties");

    public async Task<SpecialtyDto?> GetSpecialtyByIdAsync(int id)
        => await GetAsync<SpecialtyDto>($"specialties/{id}");

    public async Task AddSpecialtyAsync(SpecialtyCreateDto specialtyForCreationDto)
        => await PostAsync("specialties", specialtyForCreationDto);

    public async Task UpdateSpecialtyAsync(SpecialtyUpdateDto specialtyForUpdateDto)
        => await PutAsync("specialties", specialtyForUpdateDto);

    public async Task DeleteSpecialtyAsync(int id)
        => await DeleteAsync($"specialties/{id}");
}
