using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class DepartmentMapper
{
    public static DepartmentResponse MapToDto(this Department department)
    {
        return new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            Location = department.Location,
            CreatedAt = department.CreatedAt,
            HeadOfDepartmentId = department.HeadOfDepartmentId,
            HeadOfDepartment = department.HeadOfDepartment?.MapToDto(),
            Doctors = department.Doctors.MapToDto(),
            Nurses = department.Nurses.MapToDto(),
            Patients = department.Patients.MapToDto(),
            Rooms = department.Rooms.MapToDto()
        };
    }

    public static Department MapToDomain(this DepartmentResponse dto)
    {
        return new Department
        {
            Id = dto.Id,
            Name = dto.Name,
            Location = dto.Location,
            CreatedAt = dto.CreatedAt,
            HeadOfDepartmentId = dto.HeadOfDepartmentId,
        };
    }

    public static Department MapToDomain(this DepartmentCreateRequest dto)
    {
        return new Department
        {
            Name = dto.Name,
            Location = dto.Location,
            CreatedAt = dto.CreatedAt,
            HeadOfDepartmentId = dto.HeadOfDepartmentId,
        };
    }

    public static Department MapToDomain(this DepartmentUpdateRequest dto)
    {
        return new Department
        {
            Id = dto.Id,
            Name = dto.Name,
            Location = dto.Location,
            HeadOfDepartmentId = dto.HeadOfDepartmentId,
        };
    }

    public static DepartmentCreateRequest MapToCreationDto(this DepartmentResponse department) => new DepartmentCreateRequest
        {
            Name = department.Name,
            Location = department.Location,
            HeadOfDepartmentId = department.HeadOfDepartmentId,
            Doctors = department.Doctors,
            Nurses = department.Nurses,
            Patients = department.Patients,
            Rooms = department.Rooms
        };

    public static DepartmentUpdateRequest MapToUpdateDto(this DepartmentResponse department) => new DepartmentUpdateRequest
        {
            Id = department.Id,
            Name = department.Name,
            Location = department.Location,
            HeadOfDepartmentId = department.HeadOfDepartmentId,
            Doctors = department.Doctors,
            Nurses = department.Nurses,
            Patients = department.Patients,
            Rooms = department.Rooms
        };

    public static IEnumerable<DepartmentResponse> MapToDto(this IEnumerable<Department> departments)
    {
        return departments.Select(department => department.MapToDto()).ToList();
    }

    public static IEnumerable<Department> MapToDomain(this IEnumerable<DepartmentResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain()).ToList();
    }
}
