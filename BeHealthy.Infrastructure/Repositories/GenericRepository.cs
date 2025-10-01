using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
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

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Set<T>().AsQueryable();
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllPagedAsync(int? pageNumber = null, int? pageSize = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var query = context.Set<T>().AsQueryable();

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value);
        }

        return await query.AsNoTracking().ToListAsync();

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

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return trackChanges
            ? await context.Set<T>().Where(predicate).ToListAsync()
            : await context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = false,
        params Expression<Func<T, object>>[] includes)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<T> query = context.Set<T>();

        if (includes != null && includes.Any())
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        query = trackChanges ? query : query.AsNoTracking();

        return await query.Where(predicate).ToListAsync();
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
}
