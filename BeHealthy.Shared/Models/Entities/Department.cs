namespace BeHealthy.Shared.Models.Entities;

public class Department
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<ApplicationUser> Doctors { get; set; } = new List<ApplicationUser>();
    //public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
