using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Application.Services.Interfaces;

public interface IVisitService
{
    Task<IEnumerable<Visit>> GetAllVisitsAsync();
    Task<Visit?> GetVisitWithDetailsAsync(int visitId);
    Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId);
    Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId);
    Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId);
    Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId);
    Task<ServiceResponse> AddVisitAsync(VisitCreateRequest dto);
    Task<ServiceResponse> UpdateVisitAsync(VisitUpdateRequest dto);
    Task<ServiceResponse> DeleteVisitAsync(int id);
}