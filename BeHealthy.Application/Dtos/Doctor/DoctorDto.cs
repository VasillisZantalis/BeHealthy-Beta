using BeHealthy.Application.Dtos.Specialty;

namespace BeHealthy.Application.Dtos.Doctor;

public class DoctorDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int? DepartmentId { get; set; }
    public int? SpecialtyId { get; set; }
    public SpecialtyDto? Specialty { get; set; }
}
