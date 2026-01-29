namespace BeHealthy.Application.Interfaces.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync();
    Task<Patient?> GetPatientByUserIdAsync(string userId);
    Task DeletePatientAsync(int id);
    Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId);
}
