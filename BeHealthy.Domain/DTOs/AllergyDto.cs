namespace BeHealthy.Domain.DTOs;

public class AllergyDto
{
    public int Id { get; set; }
    public string AllergyName { get; set; } = string.Empty;
    public string? Allergen { get; set; }
    public int PatientId { get; set; }
}