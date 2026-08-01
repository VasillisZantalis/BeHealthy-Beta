namespace BeHealthy.Shared.Dtos.Nurse;

public class NurseSimpleDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => FirstName + " " + LastName;
    public string UserId { get; set; } = string.Empty;
    public string? Image { get; set; }
}
