namespace BeHealthy.Shared.Dtos.Allergy;

public class AllergyCreateDto
{
    public string AllergyName { get; set; } = string.Empty;
    public string? Allergen { get; set; }
    public AllergySeverity Severity { get; set; }
    public string? Notes { get; set; }
    public int PatientId { get; set; }
}