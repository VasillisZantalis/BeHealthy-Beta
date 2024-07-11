using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface IDoctorRepository : IGenericRepository<Doctor>
{

   // Task<Doctor> GetDoctorByUserIdAsync(string id);
}
