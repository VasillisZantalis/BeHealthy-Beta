using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IDoctorRepository : IGenericRepository<Doctor>
{

    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task DeleteDoctorAsync(int id);
}
