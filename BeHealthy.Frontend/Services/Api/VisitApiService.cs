using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Frontend.Services.Api;

public class VisitApiService : ApiClientBase, IVisitService
{
    public VisitApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<VisitResponse>> GetAllVisitsAsync()
        => await GetListAsync<VisitResponse>("visits");

    public async Task<VisitDetailsResponse?> GetVisitWithDetailsAsync(int visitId)
        => await GetAsync<VisitDetailsResponse>($"visits/{visitId}");

    public async Task<IEnumerable<DiagnosisResponse>> GetDiagnosesByVisitIdAsync(int visitId)
        => await GetListAsync<DiagnosisResponse>($"visits/{visitId}/diagnoses");

    public async Task<IEnumerable<TreatmentResponse>> GetTreatmentsByVisitIdAsync(int visitId)
        => await GetListAsync<TreatmentResponse>($"visits/{visitId}/treatments");

    public async Task<IEnumerable<LabResultResponse>> GetLabResultsByVisitIdAsync(int visitId)
        => await GetListAsync<LabResultResponse>($"visits/{visitId}/lab-results");

    public async Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId)
        => await GetListAsync<VisitResponse>($"visits/patient/{patientId}");

    public async Task<ServiceResponse> AddVisitAsync(VisitCreateRequest dto)
        => await PostForResponseAsync("visits", dto);

    public async Task<ServiceResponse> UpdateVisitAsync(VisitUpdateRequest dto)
        => await PutForResponseAsync("visits", dto);

    public async Task<ServiceResponse> DeleteVisitAsync(int id)
        => await DeleteForResponseAsync($"visits/{id}");
}
