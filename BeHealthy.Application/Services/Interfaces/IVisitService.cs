using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Application.Services.Interfaces;

public interface IVisitService
{
    Task<IEnumerable<Visit>> GetAllVisitsAsync();
    Task<Visit?> GetVisitWithDetailsAsync(int visitId);
    Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId);
    Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId);
    Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId);
    Task<IEnumerable<VisitDto>> GetVisitsByPatientIdAsync(int patientId);
    Task<ServiceResponse> AddVisitAsync(VisitCreateDto dto);
    Task<ServiceResponse> UpdateVisitAsync(VisitUpdateDto dto);
    Task<ServiceResponse> DeleteVisitAsync(int id);
}