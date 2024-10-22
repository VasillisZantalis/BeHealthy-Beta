namespace BeHealthy.Shared.Models.Dtos.Nurse;

public class NurseForCreationDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
}
