using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Services.Api;

public class AllergyApiService : ApiClientBase, IAllergyService
{
    public AllergyApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<AllergyResponse>> GetAllergiesByPatientIdAsync(int patientId)
        => await GetListAsync<AllergyResponse>($"allergies/patient/{patientId}");

    public async Task<AllergyResponse?> GetAllergyByIdAsync(int id)
        => await GetAsync<AllergyResponse>($"allergies/{id}");

    public async Task<ServiceResponse> AddAllergyAsync(AllergyCreateRequest dto)
        => await PostForResponseAsync("allergies", dto);

    public async Task<ServiceResponse> UpdateAllergyAsync(AllergyUpdateRequest dto)
        => await PutForResponseAsync("allergies", dto);

    public async Task<ServiceResponse> DeleteAllergyAsync(int id)
        => await DeleteForResponseAsync($"allergies/{id}");
}
