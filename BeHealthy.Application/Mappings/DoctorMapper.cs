using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class DoctorMapper
{
    public static DoctorResponse MapToDto(this Doctor doctor)
    {
        return new DoctorResponse
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

    public static Doctor MapToDomain(this DoctorResponse dto)
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

    public static Doctor MapToDomain(this DoctorCreateRequest dto)
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

    public static Doctor MapToDomain(this DoctorUpdateRequest dto)
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

    public static DoctorSimpleResponse MapToSimpleDto(this Doctor doctor)
    {
        return new DoctorSimpleResponse
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            UserId = doctor.UserId,
            Image = doctor.Image
        };
    }

    public static DoctorUpdateRequest MapToUpdateDto(this DoctorResponse dto)
    {
        return new DoctorUpdateRequest
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

    public static DoctorResponse MapToDto(this DoctorUpdateRequest dto)
    {
        return new DoctorResponse
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

    public static IEnumerable<DoctorResponse> MapToDto(this IEnumerable<Doctor> doctors)
    {
        return doctors.Select(d => d.MapToDto());
    }

    public static ICollection<DoctorResponse> MapToDto(this ICollection<Doctor> doctors)
    {
        return doctors.Select(d => d.MapToDto()).ToList();
    }

    public static IEnumerable<Doctor> MapToDomain(this IEnumerable<DoctorResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }

    public static IEnumerable<DoctorSimpleResponse> MapToSimpleDto(this IEnumerable<Doctor> doctors)
    {
        return doctors.Select(d => d.MapToSimpleDto());
    }
}
