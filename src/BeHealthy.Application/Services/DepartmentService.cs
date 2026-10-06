using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly INurseRepository _nurseRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IRoomRepository _roomRepository;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        IDoctorRepository doctorRepository,
        INurseRepository nurseRepository,
        IPatientRepository patientRepository,
        IRoomRepository roomRepository)
    {
        _departmentRepository = departmentRepository;
        _doctorRepository = doctorRepository;
        _nurseRepository = nurseRepository;
        _patientRepository = patientRepository;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        var departments = await _departmentRepository.GetDepartmentsAsync(cancellationToken);
        return departments.MapToDto();
    }

    public async Task<DepartmentResponse?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var department = await _departmentRepository.GetDepartmentByIdAsync(id, cancellationToken);
        return department?.MapToDto();
    }

    public async Task<ServiceResponse> AddDepartmentAsync(DepartmentCreateRequest departmentDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var department = departmentDto.MapToDomain();
            await _departmentRepository.AddAsync(department, cancellationToken);
            await _departmentRepository.SaveChangesAsync(cancellationToken);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }

    public async Task<ServiceResponse> UpdateDepartmentAsync(DepartmentUpdateRequest departmentDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var department = await _departmentRepository.GetByIdAsync(departmentDto.Id, cancellationToken);
            if (department is null)
            {
                return ServiceResponse.Failed(Resource.NotFound);
            }

            department.Name = departmentDto.Name;
            department.Location = departmentDto.Location;
            department.HeadOfDepartmentId = departmentDto.HeadOfDepartmentId;

            await _departmentRepository.UpdateAsync(department);
            await _departmentRepository.SaveChangesAsync(cancellationToken);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }

    public async Task<ServiceResponse> DeleteDepartmentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            List<string> entitiesConnectedToDepartment = new();

            if (await _doctorRepository.AnyAsync(d => d.DepartmentId == id, cancellationToken))
            {
                entitiesConnectedToDepartment.Add(Resource.Doctors);
            }

            if (await _nurseRepository.AnyAsync(n => n.DepartmentId == id, cancellationToken))
            {
                entitiesConnectedToDepartment.Add(Resource.Nurses);
            }

            if (await _patientRepository.AnyAsync(p => p.DepartmentId == id, cancellationToken))
            {
                entitiesConnectedToDepartment.Add(Resource.Patients);
            }

            if (await _roomRepository.AnyAsync(r => r.DepartmentId == id, cancellationToken))
            {
                entitiesConnectedToDepartment.Add(Resource.Rooms);
            }

            if (entitiesConnectedToDepartment.Any())
            {
                var connectedEntities = string.Join(", ", entitiesConnectedToDepartment);
                return ServiceResponse.Failed(
                    string.Format(Resource.CannotDeleteEntityWithRelationships,
                                Resource.Department,
                                connectedEntities)
                );
            }

            if (!await _departmentRepository.DeleteAsync(id, cancellationToken))
            {
                return ServiceResponse.Failed(Resource.NotFound);
            }

            await _departmentRepository.SaveChangesAsync(cancellationToken);
            return ServiceResponse.Successful();
        }
        catch (Exception)
        {
            return ServiceResponse.Failed(Resource.SomethingWentWrong);
        }
    }
}
