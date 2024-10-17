using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByUserIdAsync(string userId);
    Task DeletePatientAsync(int id);
}
