using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IVisitRepository : IGenericRepository<Visit>
{
    Task<Visit?> GetVisitWithDetailsAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Diagnosis>> GetDiagnosesByVisitIdAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetTreatmentsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LabResult>> GetLabResultsByVisitIdAsync(int visitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Visit>> GetVisitsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
}