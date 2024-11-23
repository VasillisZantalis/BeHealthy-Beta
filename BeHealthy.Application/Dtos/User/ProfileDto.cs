namespace BeHealthy.Application.Dtos.User;

public class ProfileDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Speciality { get; set; }
    public string? Image { get; set; }
    public string? PhoneNumber { get; set; }
}
