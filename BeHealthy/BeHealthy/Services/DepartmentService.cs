using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Department;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
    {
        var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task AddDepartmentAsync(DepartmentForCreationDto departmentDto)
    {
        var department = _mapper.Map<Department>(departmentDto);
        await _unitOfWork.DepartmentRepository.AddAsync(department);
    }

    public async Task UpdateDepartmentAsync(DepartmentForUpdateDto departmentDto)
    {
        var department = _mapper.Map<Department>(departmentDto);
        await _unitOfWork.DepartmentRepository.UpdateAsync(department);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await _unitOfWork.DepartmentRepository.DeleteAsync(id);
    }
}
