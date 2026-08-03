using BeHealthy.Shared.Dtos.Specialty;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class SpecialtyMapper
{
    public static SpecialtyResponse MapToDto(this Specialty specialty)
    {
        return new SpecialtyResponse
        {
            Id = specialty.Id,
            Name = specialty.Name
        };
    }

    public static Specialty MapToDomain(this SpecialtyResponse specialty)
    {
        return new Specialty
        {
            Id = specialty.Id,
            Name = specialty.Name
        };
    }

    public static Specialty MapToDomain(this SpecialtyCreateRequest specialtyForCreationDto)
    {
        return new Specialty
        {
            Name = specialtyForCreationDto.Name,
        };
    }

    public static Specialty MapToDomain(this SpecialtyUpdateRequest specialtyForUpdateDto)
    {
        return new Specialty
        {
            Id = specialtyForUpdateDto.Id,
            Name = specialtyForUpdateDto.Name,
        };
    }

    public static IEnumerable<SpecialtyResponse> MapToDto(this IEnumerable<Specialty> specialties)
    {
        return specialties.Select(s => s.MapToDto());
    }
}
