namespace BeHealthy.Domain.Entities;

public class Allergy
{
    public int Id { get; set; }
    public string AllergyName { get; set; } = string.Empty;
    public string? Allergen { get; set; }
    public AllergySeverity Severity { get; set; }
    public string? Notes { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
}
