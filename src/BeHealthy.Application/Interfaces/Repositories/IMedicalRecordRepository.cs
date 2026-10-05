namespace BeHealthy.Application.Interfaces.Repositories;

public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
{
    Task<IEnumerable<MedicalRecord>> GetMedicalRecordsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
}
