using BeHealthy.Application.Common.Models;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeHealthy.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<T?> GetByIdWithIncludes(int id, CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
    {
        var query = _context.Set<T>().AsQueryable();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity)
    {
        // Tracked entities are already change-detected; Update() would mark every column (and the whole graph) as modified.
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<T>().Update(entity);
        }

        return Task.CompletedTask;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<T>().FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return false;
        }

        _context.Set<T>().Remove(entity);
        return true;
    }

    public Task DeleteEntityAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().CountAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().CountAsync(predicate, cancellationToken);
    }

    public async Task<T?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "UserId") == userId, cancellationToken);
    }

    public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();

        if (options.Includes != null && options.Includes.Any())
        {
            query = options.Includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (!options.TrackChanges)
        {
            query = query.AsNoTracking();
        }

        if (options.Predicate != null)
        {
            query = query.Where(options.Predicate);
        }

        if (options.OrderBy != null)
        {
            query = options.OrderDescending
                ? query.OrderByDescending(options.OrderBy)
                : query.OrderBy(options.OrderBy);
        }

        if (options.PageNumber.HasValue && options.PageSize.HasValue)
        {
            var skip = (options.PageNumber.Value - 1) * options.PageSize.Value;
            query = query
                .Skip(skip)
                .Take(options.PageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
