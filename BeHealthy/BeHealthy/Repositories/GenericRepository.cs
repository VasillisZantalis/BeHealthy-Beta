using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace BeHealthy.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public GenericRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            return await contextFactory.Set<T>().ToListAsync();
        }
    }

    public async Task<IEnumerable<T>> GetAllPagedAsync(int? pageNumber = null, int? pageSize = null)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            var query = contextFactory.Set<T>().AsQueryable();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);
            }

            return await query.AsNoTracking().ToListAsync();
        }
       
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            return await contextFactory.Set<T>().FindAsync(id);
        }
    }

    public async Task AddAsync(T entity)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            await contextFactory.Set<T>().AddAsync(entity);
            await contextFactory.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(T entity)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            contextFactory.Set<T>().Update(entity);
            await contextFactory.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            var entity = await contextFactory.Set<T>().FindAsync(id);
            if (entity != null)
            {
                contextFactory.Set<T>().Remove(entity);
                await contextFactory.SaveChangesAsync();
            }
        }
        
    }

    public async Task DeleteEntityAsync(T entity)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            contextFactory.Set<T>().Remove(entity);
            await contextFactory.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        using (var contextFactory = _contextFactory.CreateDbContext())
        {
            return await contextFactory.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }
    }
}
