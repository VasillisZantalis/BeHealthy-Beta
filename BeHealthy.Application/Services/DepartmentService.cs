using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Shared.Locales;

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
            List<string> entitiesConnectedToDepartment = new();

            if (await _unitOfWork.DoctorRepository.AnyAsync(d => d.DepartmentId == id))
                entitiesConnectedToDepartment.Add(Resource.Doctors);

            if (await _unitOfWork.NurseRepository.AnyAsync(n => n.DepartmentId == id))
                entitiesConnectedToDepartment.Add(Resource.Nurses);

            if (await _unitOfWork.PatientRepository.AnyAsync(p => p.DepartmentId == id))
                entitiesConnectedToDepartment.Add(Resource.Patients);

            if (await _unitOfWork.RoomRepository.AnyAsync(r => r.DepartmentId == id))
                entitiesConnectedToDepartment.Add(Resource.Rooms);

            if (entitiesConnectedToDepartment.Any())
            {
                var connectedEntities = string.Join(", ", entitiesConnectedToDepartment);
                return ServiceResponse.Failed(
                    string.Format(Resource.CannotDeleteEntityWithRelationships, 
                                Resource.Department,
                                connectedEntities)
                );
            }

            await _unitOfWork.DepartmentRepository.DeleteAsync(id);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }
}
