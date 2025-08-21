using BeHealthy.Application.Dtos.Allergy;

namespace BeHealthy.Application.Mappings;

public static class AllergyMapper
{
    public static Allergy ToEntity(this AllergyCreateDto dto)
    {
        return new Allergy
        {
            AllergyName = dto.AllergyName,
            Allergen = dto.Allergen,
            PatientId = dto.PatientId
        };
    }

    public static void MapToEntity(this AllergyUpdateDto dto, Allergy entity)
    {
        entity.AllergyName = dto.AllergyName;
        entity.Allergen = dto.Allergen;
        entity.PatientId = dto.PatientId;
    }

    public static AllergyUpdateDto ToUpdateDto(this Allergy entity)
    {
        return new AllergyUpdateDto
        {
            Id = entity.Id,
            AllergyName = entity.AllergyName,
            Allergen = entity.Allergen,
            PatientId = entity.PatientId
        };
    }
}