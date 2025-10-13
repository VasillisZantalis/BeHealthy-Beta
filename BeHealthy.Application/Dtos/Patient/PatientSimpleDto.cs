namespace BeHealthy.Application.Dtos.Patient;

public class PatientSimpleDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => FirstName + " " + LastName;
    public string UserId { get; set; } = string.Empty;
    public string? Image { get; set; }
}
