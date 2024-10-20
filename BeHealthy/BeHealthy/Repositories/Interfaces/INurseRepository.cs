using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface INurseRepository : IGenericRepository<Nurse>
{
    Task<IEnumerable<Nurse>> GetAllNursesAsync();
    Task DeleteNurseAsync(int id);
}
