namespace BeHealthy.Shared.Dtos.Doctor;

public class DoctorCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int? SpecialtyId { get; set; }
    public string? PhoneNumber { get; set; }
}
