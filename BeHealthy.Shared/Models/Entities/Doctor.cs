namespace BeHealthy.Shared.Models.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Specialty { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = new();

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = new();

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

