using BeHealthy.Application.Dtos.Specialty;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class SpecialtyMapper
{
    public static SpecialtyDto MapToDto(this Specialty Specialty)
    {
        return new SpecialtyDto
        {
            Id = Specialty.Id,
            Name = Specialty.Name
        };
    }

    public static Specialty MapToDomain(this SpecialtyDto Specialty)
    {
        return new Specialty
        {
            Id = Specialty.Id,
            Name = Specialty.Name
        };
    }
}
