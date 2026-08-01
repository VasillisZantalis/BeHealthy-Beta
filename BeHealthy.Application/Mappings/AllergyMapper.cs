using BeHealthy.Shared.Dtos.Allergy;

namespace BeHealthy.Application.Mappings;

public static class AllergyMapper
{
    public static AllergyDto MapToDto(this Allergy allergy)
    {
        return new AllergyDto
        {
            Id = allergy.Id,
            AllergyName = allergy.AllergyName,
            Allergen = allergy.Allergen,
            Severity = allergy.Severity,
            Notes = allergy.Notes,
            PatientId = allergy.PatientId
        };
    }

    public static Allergy MapToDomain(this AllergyCreateDto dto)
    {
        return new Allergy
        {
            AllergyName = dto.AllergyName,
            Allergen = dto.Allergen,
            Severity = dto.Severity,
            Notes = dto.Notes,
            PatientId = dto.PatientId
        };
    }

    public static Allergy MapToDomain(this AllergyUpdateDto dto)
    {
        return new Allergy
        {
            Id = dto.Id,
            AllergyName = dto.AllergyName,
            Allergen = dto.Allergen,
            Severity = dto.Severity,
            Notes = dto.Notes,
            PatientId = dto.PatientId
        };
    }

    public static AllergyCreateDto MapToCreateDto(this AllergyDto dto)
    {
        return new AllergyCreateDto
        {
            AllergyName = dto.AllergyName,
            Allergen = dto.Allergen,
            Severity = dto.Severity,
            Notes = dto.Notes,
            PatientId = dto.PatientId
        };
    }

    public static AllergyUpdateDto MapToUpdateDto(this AllergyDto dto)
    {
        return new AllergyUpdateDto
        {
            Id = dto.Id,
            AllergyName = dto.AllergyName,
            Allergen = dto.Allergen,
            Severity = dto.Severity,
            Notes = dto.Notes,
            PatientId = dto.PatientId
        };
    }
}