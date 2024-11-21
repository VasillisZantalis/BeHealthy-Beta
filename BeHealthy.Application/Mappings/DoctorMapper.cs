using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class DoctorMapper
{
    public static DoctorDto MapToDto(this Doctor doctor)
    {
        return new DoctorDto
        {
            Id = doctor.Id,
            UserId = doctor.UserId,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Image = doctor.Image,
            Specialty = doctor.Specialty,
            PhoneNumber = doctor.User?.PhoneNumber ?? string.Empty,
            Email = doctor.User?.Email ?? string.Empty,
            CreatedAt = doctor.CreatedAt,
            DepartmentId = doctor.DepartmentId
        };
    }

    public static Doctor MapToDomain(this DoctorDto dto)
    {
        return new Doctor
        {
            Id = dto.Id,
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            Specialty = dto.Specialty,
            CreatedAt = dto.CreatedAt,
            DepartmentId = dto.DepartmentId
        };
    }

    public static Doctor MapToDomain(this DoctorForCreationDto dto)
    {
        return new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Specialty = dto.Specialty,
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId,
            Image = dto.Image
        };
    }

    public static Doctor MapToDomain(this DoctorForUpdateDto dto)
    {
        return new Doctor
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            Specialty = dto.Specialty,
            DepartmentId = dto.DepartmentId
        };
    }

    public static DoctorSimpleDto MapToSimpleDto(this Doctor doctor)
    {
        return new DoctorSimpleDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Image = doctor.Image
        };
    }

    public static DoctorForUpdateDto MapToUpdateDto(this DoctorDto dto)
    {
        return new DoctorForUpdateDto
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            Specialty = dto.Specialty,
            PhoneNumber = dto.PhoneNumber,
            DepartmentId = dto.DepartmentId
        };
    }

    public static DoctorDto MapToDto(this DoctorForUpdateDto dto)
    {
        return new DoctorDto
        {
            Id = dto.Id,
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            Specialty = dto.Specialty,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            DepartmentId = dto.DepartmentId
        };
    }

    public static IEnumerable<DoctorDto> MapToDto(this IEnumerable<Doctor> doctors)
    {
        return doctors.Select(d => d.MapToDto());
    }

    public static ICollection<DoctorDto> MapToDto(this ICollection<Doctor> doctors)
    {
        return doctors.Select(d => d.MapToDto()).ToList();
    }

    public static IEnumerable<Doctor> MapToDomain(this IEnumerable<DoctorDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }

    public static IEnumerable<DoctorSimpleDto> MapToSimpleDto(this IEnumerable<Doctor> doctors)
    {
        return doctors.Select(d => d.MapToSimpleDto());
    }
}
