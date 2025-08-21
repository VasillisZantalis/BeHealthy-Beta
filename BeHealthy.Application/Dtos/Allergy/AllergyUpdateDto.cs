namespace BeHealthy.Application.Dtos.Allergy;

public class AllergyUpdateDto
{
    public int Id { get; set; }
    public string AllergyName { get; set; } = string.Empty;
    public string? Allergen { get; set; }
    public int PatientId { get; set; }
}