namespace BeHealthy.Application.Interfaces.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync(CancellationToken cancellationToken = default);
    Task<Patient?> GetPatientByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task DeletePatientAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken = default);
}
