namespace BeHealthy.Application.Dtos.Nurse;

public class NurseCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
}
