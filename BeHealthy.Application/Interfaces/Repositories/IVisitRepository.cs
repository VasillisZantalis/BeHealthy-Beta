using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IVisitRepository : IGenericRepository<Visit>
{
    Task<Visit?> GetVisitWithDetailsAsync(int visitId);
    Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId);
    Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId);
    Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId);
    Task<IEnumerable<Visit>> GetVisitsByPatientIdAsync(int patientId);
}