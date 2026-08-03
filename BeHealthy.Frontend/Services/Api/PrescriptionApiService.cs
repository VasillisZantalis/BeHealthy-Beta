using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Frontend.Services.Api;

public class PrescriptionApiService : ApiClientBase, IPrescriptionService
{
    public PrescriptionApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync()
        => await GetListAsync<PrescriptionResponse>("prescriptions");

    public async Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id)
        => await GetAsync<PrescriptionResponse>($"prescriptions/{id}");

    public async Task<IEnumerable<PrescriptionResponse>> GetPrescriptionsByPatientIdAsync(int id)
        => await GetListAsync<PrescriptionResponse>($"prescriptions/patient/{id}");

    public async Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateRequest prescriptionDto)
        => await PostForResponseAsync("prescriptions", prescriptionDto);

    public async Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateRequest prescriptionDto)
        => await PutForResponseAsync("prescriptions", prescriptionDto);

    public async Task<ServiceResponse> DeletePrescriptionAsync(int id)
        => await DeleteForResponseAsync($"prescriptions/{id}");
}
