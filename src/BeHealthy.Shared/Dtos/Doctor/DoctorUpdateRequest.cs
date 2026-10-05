namespace BeHealthy.Shared.Dtos.Doctor;

public class DoctorUpdateRequest
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public int? SpecialtyId { get; set; }
}
