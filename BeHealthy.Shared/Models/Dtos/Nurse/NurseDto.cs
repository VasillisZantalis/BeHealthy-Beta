namespace BeHealthy.Shared.Models.Dtos.Nurse;

public class NurseDto
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
