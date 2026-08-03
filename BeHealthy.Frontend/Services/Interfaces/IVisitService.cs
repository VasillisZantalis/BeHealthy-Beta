using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IVisitService
{
    Task<IEnumerable<VisitResponse>> GetAllVisitsAsync();
    Task<VisitDetailsResponse?> GetVisitWithDetailsAsync(int visitId);
    Task<IEnumerable<DiagnosisResponse>> GetDiagnosesByVisitIdAsync(int visitId);
    Task<IEnumerable<TreatmentResponse>> GetTreatmentsByVisitIdAsync(int visitId);
    Task<IEnumerable<LabResultResponse>> GetLabResultsByVisitIdAsync(int visitId);
    Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId);
    Task<ServiceResponse> AddVisitAsync(VisitCreateRequest dto);
    Task<ServiceResponse> UpdateVisitAsync(VisitUpdateRequest dto);
    Task<ServiceResponse> DeleteVisitAsync(int id);
}
