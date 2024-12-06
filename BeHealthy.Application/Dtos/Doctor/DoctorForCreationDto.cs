namespace BeHealthy.Application.Dtos.Doctor;

public class DoctorForCreationDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string? Image { get; set; }
    public int? SpecialtyId { get; set; }
}
