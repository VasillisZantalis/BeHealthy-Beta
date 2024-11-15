namespace BeHealthy.Application.Dtos.Patient;

public class PatientSimpleDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? Image { get; set; }
}
