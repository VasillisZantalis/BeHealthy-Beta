using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto> GetDepartmentByIdAsync(int id);
    Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateDto departmentDto);
    Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateDto departmentDto);
    Task<ServiceResponse> DeleteDepartmentAsync(int id);
}
