using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync(string? firstName = null, string? lastName = null);
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task DeletePatientAsync(int id);
}
