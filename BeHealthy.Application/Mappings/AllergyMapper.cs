using BeHealthy.Shared.Dtos.Allergy;

namespace BeHealthy.Application.Mappings;

public static class AllergyMapper
{
    public static AllergyResponse MapToDto(this Allergy allergy)
    {
        return new AllergyResponse
        {
            Id = allergy.Id,
            AllergyName = allergy.AllergyName,
            Allergen = allergy.Allergen,
            Severity = allergy.Severity,
            Notes = allergy.Notes,
            PatientId = allergy.PatientId
        };
    }

    public static Allergy MapToDomain(this AllergyCreateRequest dto)
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

    public static Allergy MapToDomain(this AllergyUpdateRequest dto)
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

    public static AllergyCreateRequest MapToCreateDto(this AllergyResponse dto)
    {
        return new AllergyCreateRequest
        {
            AllergyName = dto.AllergyName,
            Allergen = dto.Allergen,
            Severity = dto.Severity,
            Notes = dto.Notes,
            PatientId = dto.PatientId
        };
    }

    public static AllergyUpdateRequest MapToUpdateDto(this AllergyResponse dto)
    {
        return new AllergyUpdateRequest
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