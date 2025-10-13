using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Departments
                    .AsNoTracking()
                    .Include(d => d.HeadOfDepartment)
                    .ToListAsync();
    }

    public async Task<Department> GetDepartmentByIdAsync(int departmentId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Departments
                    .AsNoTracking()
                    .Include(d => d.Doctors)
                    .ThenInclude(doc => doc.User)
                    .Include(d => d.Doctors)
                    .ThenInclude(doc => doc.Specialty)
                    .Include(d => d.Patients)
                    .ThenInclude(p => p.User)
                    .Include(d => d.Nurses)
                    .ThenInclude(n => n.User)
                    .Include(d => d.Rooms)
                    .Include(d => d.HeadOfDepartment)
                    .ThenInclude(hd => hd != null ? hd.User : null)
                    .FirstAsync(x => x.Id == departmentId);
    }
}
