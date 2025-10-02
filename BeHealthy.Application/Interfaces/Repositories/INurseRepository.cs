using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface INurseRepository : IGenericRepository<Nurse>
{
    Task<IEnumerable<Nurse>> GetAllNursesAsync();
    Task DeleteNurseAsync(int id);
}
