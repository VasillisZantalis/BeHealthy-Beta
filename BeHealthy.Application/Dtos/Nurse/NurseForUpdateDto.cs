namespace BeHealthy.Application.Dtos.Nurse;

public class NurseForUpdateDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
}
