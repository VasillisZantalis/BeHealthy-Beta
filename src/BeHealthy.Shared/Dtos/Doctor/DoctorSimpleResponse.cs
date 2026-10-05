namespace BeHealthy.Shared.Dtos.Doctor;

public class DoctorSimpleResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => FirstName + " " + LastName;
    public string UserId { get; set; } = string.Empty;
    public string? Image { get; set; }
}
