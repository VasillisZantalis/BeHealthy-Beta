using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IVisitService
{
    Task<IEnumerable<VisitDto>> GetAllVisitsAsync();
    Task<VisitDetailsDto?> GetVisitWithDetailsAsync(int visitId);
    Task<IEnumerable<DiagnosisDto>> GetDiagnosesByVisitIdAsync(int visitId);
    Task<IEnumerable<TreatmentDto>> GetTreatmentsByVisitIdAsync(int visitId);
    Task<IEnumerable<LabResultDto>> GetLabResultsByVisitIdAsync(int visitId);
    Task<IEnumerable<VisitDto>> GetVisitsByPatientIdAsync(int patientId);
    Task<ServiceResponse> AddVisitAsync(VisitCreateDto dto);
    Task<ServiceResponse> UpdateVisitAsync(VisitUpdateDto dto);
    Task<ServiceResponse> DeleteVisitAsync(int id);
}
