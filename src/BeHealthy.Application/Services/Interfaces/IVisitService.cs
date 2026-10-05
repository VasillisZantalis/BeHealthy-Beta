using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Application.Services.Interfaces;

public interface IVisitService
{
    Task<IEnumerable<Visit>> GetAllVisitsAsync(CancellationToken cancellationToken = default);
    Task<Visit?> GetVisitWithDetailsAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddVisitAsync(VisitCreateRequest dto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateVisitAsync(VisitUpdateRequest dto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteVisitAsync(int id, CancellationToken cancellationToken = default);
}