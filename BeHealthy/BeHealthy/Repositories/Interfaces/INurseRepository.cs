using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Repositories.Interfaces;

public interface INurseRepository : IGenericRepository<Nurse>
{
    Task DeleteNurseAsync(int id);
}
