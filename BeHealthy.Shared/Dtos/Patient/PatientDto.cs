namespace BeHealthy.Shared.Dtos.Patient;

public class PatientDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FullName => FirstName + " " + LastName;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int? DepartmentId { get; set; }
}
