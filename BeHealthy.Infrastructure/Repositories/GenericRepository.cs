using BeHealthy.Application.Common.Models;
using BeHealthy.Application.Interfaces.Repositories;
using BeHealthy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeHealthy.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public GenericRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().FindAsync(id);
    }

    public IQueryable<T> GetQueryable()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Set<T>().AsQueryable();
    }

    public async Task<T?> GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Set<T>().AsQueryable();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task AddAsync(T entity)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        await context.Set<T>()
            .Where(e => EF.Property<int>(e, "Id") == id)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteEntityAsync(T entity)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<int> GetCountAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().CountAsync();
    }

    public async Task<T?> GetByUserIdAsync(string userId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().FirstOrDefaultAsync(e => EF.Property<string>(e, "UserId") == userId);
    }

    public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<T> query = context.Set<T>();

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
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().AnyAsync(predicate);
    }
}
