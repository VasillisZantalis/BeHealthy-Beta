using BeHealthy.Application.Common.Models;
using System.Linq.Expressions;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options);
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByUserIdAsync(string userId);
    Task<T?> GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task DeleteEntityAsync(T entity);
    Task<bool> ExistsAsync(int id);
    Task<int> GetCountAsync();
}
