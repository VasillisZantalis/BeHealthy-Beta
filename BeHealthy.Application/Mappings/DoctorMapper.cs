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
            SpecialtyId = doctor.SpecialtyId,
            Specialty = doctor.Specialty?.MapToDto(),
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
            SpecialtyId = dto.SpecialtyId,
            CreatedAt = dto.CreatedAt,
            DepartmentId = dto.DepartmentId
        };
    }

    public static Doctor MapToDomain(this DoctorCreateDto dto)
    {
        return new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            SpecialtyId = dto.SpecialtyId,
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId,
            Image = dto.Image,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Doctor MapToDomain(this DoctorUpdateDto dto)
    {
        return new Doctor
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            SpecialtyId = dto.SpecialtyId,
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
            UserId = doctor.UserId,
            Image = doctor.Image
        };
    }

    public static DoctorUpdateDto MapToUpdateDto(this DoctorDto dto)
    {
        return new DoctorUpdateDto
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            UserId = dto.UserId,
            SpecialtyId = dto.SpecialtyId,
            PhoneNumber = dto.PhoneNumber,
            DepartmentId = dto.DepartmentId
        };
    }

    public static DoctorDto MapToDto(this DoctorUpdateDto dto)
    {
        return new DoctorDto
        {
            Id = dto.Id,
            UserId = dto.UserId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Image = dto.Image,
            SpecialtyId = dto.SpecialtyId,
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
