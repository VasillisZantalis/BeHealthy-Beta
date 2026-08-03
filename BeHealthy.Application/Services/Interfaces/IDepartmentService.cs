using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
    Task<DepartmentResponse> GetDepartmentByIdAsync(int id);
    Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateRequest departmentDto);
    Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateRequest departmentDto);
    Task<ServiceResponse> DeleteDepartmentAsync(int id);
}
