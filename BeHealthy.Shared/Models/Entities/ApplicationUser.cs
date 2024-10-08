using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Shared.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public int? DoctorId { get; set; }
    public virtual Doctor? Doctor { get; set; }

    public int? PatientId { get; set; }
    public virtual Patient? Patient { get; set; }

    public int? NurseId { get; set; }
    public virtual Nurse? Nurse { get; set; }
}
