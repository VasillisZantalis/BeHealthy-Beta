using BeHealthy.Application.Dtos.Department;
using BeHealthy.Shared.Locales;
using BeHealthy.Application.Interfaces;

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

    public async Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateDto departmentDto)
    {
        try
        {
            var department = departmentDto.MapToDomain();
            await _unitOfWork.DepartmentRepository.AddAsync(department);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }

    public async Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateDto departmentDto)
    {
        try
        {
            var department = departmentDto.MapToDomain();
            await _unitOfWork.DepartmentRepository.UpdateAsync(department);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }

    public async Task<ServiceResponse> DeleteDepartmentAsync(int id)
    {
        try
        {
            await _unitOfWork.DepartmentRepository.DeleteAsync(id);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }
}
