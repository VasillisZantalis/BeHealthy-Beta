using BeHealthy.Domain.Entities;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{

    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
    Task<IEnumerable<Doctor>> GetAllDoctorsSimpleAsync();
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsByUserIdAsync(string userId);
    Task<Doctor?> GetDoctorByUserIdAsync(string userId);
    Task DeleteDoctorAsync(int id);
    Task<bool> IsDoctorHeadOfDepartmentAsync(int doctorId);
}
