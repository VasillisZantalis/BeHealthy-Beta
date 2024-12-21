using System.Linq.Expressions;

namespace BeHealthy.Domain.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllPagedAsync(int? pageNumber = null, int? pageSize = null);
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> GetQueryable();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task DeleteEntityAsync(T entity);
    Task<bool> ExistsAsync(int id);
    Task<int> GetCountAsync();
}
