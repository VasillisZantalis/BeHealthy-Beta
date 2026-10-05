using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Specialty;

namespace BeHealthy.Front.Services.Api;

public class SpecialtyApiService : ApiClientBase, ISpecialtyService
{
    public SpecialtyApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<IEnumerable<SpecialtyResponse>> GetSpecialtiesAsync()
        => await GetListAsync<SpecialtyResponse>("specialties");

    public async Task<SpecialtyResponse?> GetSpecialtyByIdAsync(int id)
        => await GetAsync<SpecialtyResponse>($"specialties/{id}");

    public async Task<ServiceResponse> AddSpecialtyAsync(SpecialtyCreateRequest specialtyForCreationDto)
        => await PostForResponseAsync("specialties", specialtyForCreationDto);

    public async Task<ServiceResponse> UpdateSpecialtyAsync(SpecialtyUpdateRequest specialtyForUpdateDto)
        => await PutForResponseAsync($"specialties/{specialtyForUpdateDto.Id}", specialtyForUpdateDto);

    public async Task DeleteSpecialtyAsync(int id)
        => await DeleteAsync($"specialties/{id}");
}
