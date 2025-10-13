using BeHealthy.Domain.Entities;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task<IEnumerable<Patient>> GetAllPatientsSimpleAsync();
    Task DeletePatientAsync(int id);
    Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId);
}
