namespace BeHealthy.Application.Interfaces.Repositories;

public interface INurseRepository : IGenericRepository<Nurse>
{
    Task<IEnumerable<Nurse>> GetAllNursesAsync();
    Task<Nurse?> GetNurseByUserIdAsync(string userId);
    Task DeleteNurseAsync(int id);
}
