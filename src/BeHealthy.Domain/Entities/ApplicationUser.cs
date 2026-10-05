using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public virtual Doctor? Doctor { get; set; }
    public virtual Patient? Patient { get; set; }
    public virtual Nurse? Nurse { get; set; }
}
