using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Frontend.Services.Api;

public class PrescriptionApiService : ApiClientBase, IPrescriptionService
{
    public PrescriptionApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
        => await GetListAsync<PrescriptionDto>("prescriptions");

    public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id)
        => await GetAsync<PrescriptionDto>($"prescriptions/{id}");

    public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int id)
        => await GetListAsync<PrescriptionDto>($"prescriptions/patient/{id}");

    public async Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateDto prescriptionDto)
        => await PostForResponseAsync("prescriptions", prescriptionDto);

    public async Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateDto prescriptionDto)
        => await PutForResponseAsync("prescriptions", prescriptionDto);

    public async Task<ServiceResponse> DeletePrescriptionAsync(int id)
        => await DeleteForResponseAsync($"prescriptions/{id}");
}
