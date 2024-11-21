using BeHealthy.Application.Mappings;
using BeHealthy.Application.Dtos.Department;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces;

namespace BeHealthy.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _unitOfWork.DepartmentRepository.GetDepartmentsAsync();
        return departments.MapToDto();
    }

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _unitOfWork.DepartmentRepository.GetDepartmentByIdAsync(id);
        return department.MapToDto();
    }

    public async Task AddDepartmentAsync(DepartmentForCreationDto departmentDto)
    {
        var department = departmentDto.MapToDomain();
        await _unitOfWork.DepartmentRepository.AddAsync(department);
    }

    public async Task UpdateDepartmentAsync(DepartmentForUpdateDto departmentDto)
    {
        var department = departmentDto.MapToDomain();
        await _unitOfWork.DepartmentRepository.UpdateAsync(department);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await _unitOfWork.DepartmentRepository.DeleteAsync(id);
    }
}
