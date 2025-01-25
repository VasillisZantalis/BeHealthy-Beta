using BeHealthy.Domain.Entities;
using BeHealthy.Shared.Parameters;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync(PatientSearchingParameters patientSearchingParameters);
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task DeletePatientAsync(int id);
    Task<IEnumerable<Patient>> GetPatientsByDepartmentIdAsync(int departmentId);
}
