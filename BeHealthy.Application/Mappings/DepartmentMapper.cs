using BeHealthy.Application.Dtos.Department;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class DepartmentMapper
{
    public static DepartmentDto MapToDto(this Department department)
    {
        return new DepartmentDto
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

    public static Department MapToDomain(this DepartmentDto dto)
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

    public static Department MapToDomain(this DepartmentCreateDto dto)
    {
        return new Department
        {
            Name = dto.Name,
            Location = dto.Location,
            CreatedAt = dto.CreatedAt,
            HeadOfDepartmentId = dto.HeadOfDepartmentId,
        };
    }

    public static Department MapToDomain(this DepartmentUpdateDto dto)
    {
        return new Department
        {
            Id = dto.Id,
            Name = dto.Name,
            Location = dto.Location,
            HeadOfDepartmentId = dto.HeadOfDepartmentId,
        };
    }

    public static DepartmentCreateDto MapToCreationDto(this DepartmentDto department) => new DepartmentCreateDto
        {
            Name = department.Name,
            Location = department.Location,
            HeadOfDepartmentId = department.HeadOfDepartmentId,
            Doctors = department.Doctors,
            Nurses = department.Nurses,
            Patients = department.Patients,
            Rooms = department.Rooms
        };

    public static DepartmentUpdateDto MapToUpdateDto(this DepartmentDto department) => new DepartmentUpdateDto
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

    public static IEnumerable<DepartmentDto> MapToDto(this IEnumerable<Department> departments)
    {
        return departments.Select(department => department.MapToDto()).ToList();
    }

    public static IEnumerable<Department> MapToDomain(this IEnumerable<DepartmentDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain()).ToList();
    }
}
