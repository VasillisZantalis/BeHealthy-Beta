namespace BeHealthy.Application.Interfaces.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync(CancellationToken cancellationToken = default);
    Task<Patient?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeletePatientAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>True when the patient has appointments, visits, medical records, prescriptions or allergies, which must be kept.</summary>
    Task<bool> HasClinicalHistoryAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default);
}
