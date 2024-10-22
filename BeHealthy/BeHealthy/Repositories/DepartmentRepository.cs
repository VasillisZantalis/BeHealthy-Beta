using BeHealthy.Data;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeHealthy.Repositories;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Departments
                    .Include(d => d.Doctors)
                    .Include(d => d.Patients)
                    .Include(d => d.Nurses)
                    .Include(d => d.Rooms)
                    .Include(d => d.HeadOfDepartment)
                    .ToListAsync();
        }
    }

    public async Task<Department> GetDepartmentByIdAsync(int departmentId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Departments
                    .Include(d => d.Doctors)
                    .Include(d => d.Patients)
                    .Include(d => d.Nurses)
                    .Include(d => d.Rooms)
                    .Include(d => d.HeadOfDepartment)
                    .FirstAsync(x => x.Id == departmentId);
        }
    }
}
