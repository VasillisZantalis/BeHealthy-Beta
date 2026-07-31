using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Api;

public class AllergyApiService : ApiClientBase, IAllergyService
{
    public AllergyApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<AllergyDto>> GetAllergiesByPatientIdAsync(int patientId)
        => await GetListAsync<AllergyDto>($"allergies/patient/{patientId}");

    public async Task<AllergyDto?> GetAllergyByIdAsync(int id)
        => await GetAsync<AllergyDto>($"allergies/{id}");

    public async Task<ServiceResponse> AddAllergyAsync(AllergyCreateDto dto)
        => await PostForResponseAsync("allergies", dto);

    public async Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateDto dto)
        => await PutForResponseAsync("allergies", dto);

    public async Task<ServiceResponse> DeleteAllergyAsync(int id)
        => await DeleteForResponseAsync($"allergies/{id}");
}
