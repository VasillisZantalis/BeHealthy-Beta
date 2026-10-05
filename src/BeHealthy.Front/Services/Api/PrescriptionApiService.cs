using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Prescription;

namespace BeHealthy.Front.Services.Api;

public class PrescriptionApiService : ApiClientBase, IPrescriptionService
{
    public PrescriptionApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<IEnumerable<PrescriptionResponse>> GetAllPrescriptionsAsync()
        => await GetListAsync<PrescriptionResponse>("prescriptions");

    public async Task<PrescriptionResponse?> GetPrescriptionByIdAsync(int id)
        => await GetAsync<PrescriptionResponse>($"prescriptions/{id}");

    public async Task<IEnumerable<PrescriptionResponse>> GetPrescriptionsByPatientIdAsync(int id)
        => await GetListAsync<PrescriptionResponse>($"patients/{id}/prescriptions");

    public async Task<ServiceResponse> AddPrescriptionAsync(PrescriptionCreateRequest prescriptionDto)
        => await PostForResponseAsync("prescriptions", prescriptionDto);

    public async Task<ServiceResponse> UpdatePrescriptionAsync(PrescriptionUpdateRequest prescriptionDto)
        => await PutForResponseAsync($"prescriptions/{prescriptionDto.Id}", prescriptionDto);

    public async Task<ServiceResponse> DeletePrescriptionAsync(int id)
        => await DeleteForResponseAsync($"prescriptions/{id}");
}
