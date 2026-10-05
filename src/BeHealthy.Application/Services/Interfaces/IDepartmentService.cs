using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Department;

namespace BeHealthy.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<DepartmentResponse> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateRequest departmentDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateRequest departmentDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default);
}
