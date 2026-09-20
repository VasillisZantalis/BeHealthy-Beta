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

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public IQueryable<T> GetQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<T?> GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes)
    {
        var query = _context.Set<T>().AsQueryable();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Set<T>()
            .Where(e => EF.Property<int>(e, "Id") == id)
            .ExecuteDeleteAsync();
    }

    public Task DeleteEntityAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Set<T>().CountAsync();
    }

    public async Task<T?> GetByUserIdAsync(string userId)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "UserId") == userId);
    }

    public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options)
    {
        IQueryable<T> query = _context.Set<T>();

        if (options.Includes != null && options.Includes.Any())
            query = options.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (!options.TrackChanges)
            query = query.AsNoTracking();

        if (options.Predicate != null)
            query = query.Where(options.Predicate);

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

        return await query.ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AnyAsync(predicate);
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
