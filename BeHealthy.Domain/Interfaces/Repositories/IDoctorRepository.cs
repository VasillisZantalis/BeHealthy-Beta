using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{

    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task<Doctor?> GetDoctorByUserIdAsync(string userId);
    Task DeleteDoctorAsync(int id);
}
