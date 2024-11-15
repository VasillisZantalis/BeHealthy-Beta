using BeHealthy.Application.Dtos.Department;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto> GetDepartmentByIdAsync(int id);
    Task AddDepartmentAsync(DepartmentForCreationDto departmentDto);
    Task UpdateDepartmentAsync(DepartmentForUpdateDto departmentDto);
    Task DeleteDepartmentAsync(int id);
}
