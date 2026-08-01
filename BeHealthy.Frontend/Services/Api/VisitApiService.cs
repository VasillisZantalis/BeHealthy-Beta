using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Frontend.Services.Api;

public class VisitApiService : ApiClientBase, IVisitService
{
    public VisitApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<VisitDto>> GetAllVisitsAsync()
        => await GetListAsync<VisitDto>("visits");

    public async Task<VisitDetailsDto?> GetVisitWithDetailsAsync(int visitId)
        => await GetAsync<VisitDetailsDto>($"visits/{visitId}");

    public async Task<IEnumerable<DiagnosisDto>> GetDiagnosesByVisitIdAsync(int visitId)
        => await GetListAsync<DiagnosisDto>($"visits/{visitId}/diagnoses");

    public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByVisitIdAsync(int visitId)
        => await GetListAsync<TreatmentDto>($"visits/{visitId}/treatments");

    public async Task<IEnumerable<LabResultDto>> GetLabResultsByVisitIdAsync(int visitId)
        => await GetListAsync<LabResultDto>($"visits/{visitId}/lab-results");

    public async Task<IEnumerable<VisitDto>> GetVisitsByPatientIdAsync(int patientId)
        => await GetListAsync<VisitDto>($"visits/patient/{patientId}");

    public async Task<ServiceResponse> AddVisitAsync(VisitCreateDto dto)
        => await PostForResponseAsync("visits", dto);

    public async Task<ServiceResponse> UpdateVisitAsync(VisitUpdateDto dto)
        => await PutForResponseAsync("visits", dto);

    public async Task<ServiceResponse> DeleteVisitAsync(int id)
        => await DeleteForResponseAsync($"visits/{id}");
}
