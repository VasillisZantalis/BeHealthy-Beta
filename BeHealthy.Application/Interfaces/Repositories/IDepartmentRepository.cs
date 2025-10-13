using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IEnumerable<Department>> GetDepartmentsAsync();
    Task<Department> GetDepartmentByIdAsync(int departmentId);
}
