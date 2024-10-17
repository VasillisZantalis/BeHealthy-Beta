using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IDoctorRepository : IGenericRepository<Doctor>
{

    // Task<Doctor> GetDoctorByUserIdAsync(string id);
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task DeleteDoctorAsync(int id);
}
