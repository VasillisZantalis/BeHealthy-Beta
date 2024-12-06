using BeHealthy.Application.Dtos.Specialty;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class SpecialtyMapper
{
    public static SpecialtyDto MapToDto(this Specialty specialty)
    {
        return new SpecialtyDto
        {
            Id = specialty.Id,
            Name = specialty.Name
        };
    }

    public static Specialty MapToDomain(this SpecialtyDto specialty)
    {
        return new Specialty
        {
            Id = specialty.Id,
            Name = specialty.Name
        };
    }

    public static Specialty MapToDomain(this SpecialtyForCreationDto specialtyForCreationDto)
    {
        return new Specialty
        {
            Name = specialtyForCreationDto.Name,
        };
    }

    public static Specialty MapToDomain(this SpecialtyForUpdateDto specialtyForUpdateDto)
    {
        return new Specialty
        {
            Id = specialtyForUpdateDto.Id,
            Name = specialtyForUpdateDto.Name,
        };
    }

    public static IEnumerable<SpecialtyDto> MapToDto(this IEnumerable<Specialty> specialties)
    {
        return specialties.Select(s => s.MapToDto());
    }
}
