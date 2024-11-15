namespace BeHealthy.Application.Dtos.Doctor;

public class DoctorSimpleDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? Image { get; set; }
}
