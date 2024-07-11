using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Shared.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<Appointment> AppointmentsAsPatient { get; set; } = new HashSet<Appointment>();
    public virtual ICollection<Appointment> AppointmentsAsDoctor { get; set; } = new HashSet<Appointment>();
    public virtual ICollection<Prescription> PrescriptionAsPatient { get; set; } = new HashSet<Prescription>();
    public virtual ICollection<Prescription> PrescriptionAsDoctor { get; set; } = new HashSet<Prescription>();
}
