namespace BeHealthy.Application.Interfaces.Repositories;

public interface INurseRepository : IGenericRepository<Nurse>
{
    Task<IEnumerable<Nurse>> GetAllNursesAsync(CancellationToken cancellationToken = default);
    Task<Nurse?> GetNurseByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeleteNurseAsync(int id, CancellationToken cancellationToken = default);
}
