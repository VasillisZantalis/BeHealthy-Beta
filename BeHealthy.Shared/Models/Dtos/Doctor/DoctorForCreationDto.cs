namespace BeHealthy.Shared.Models.Dtos.Doctor;

public class DoctorForCreationDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
