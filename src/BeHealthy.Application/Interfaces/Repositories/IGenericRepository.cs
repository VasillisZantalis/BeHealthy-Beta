namespace BeHealthy.Application.Interfaces.Repositories;

/// <summary>
/// Repositories only stage changes on the request-scoped DbContext; nothing is written until
/// <see cref="SaveChangesAsync"/> is called. All repositories share that DbContext, so a save
/// persists every pending change of the request atomically.
/// </summary>
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<T?> GetByIdWithIncludes(int id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity);
    /// <returns><c>false</c> when no entity with that id exists.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteEntityAsync(T entity);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
