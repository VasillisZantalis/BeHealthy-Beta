namespace BeHealthy.Shared.Models.Dtos.Patient;

public class PatientDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
}
