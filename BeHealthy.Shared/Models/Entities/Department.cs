namespace BeHealthy.Shared.Models.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty ;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? HeadOfDepartmentId { get; set; }
    public Doctor? HeadOfDepartment { get; set; }
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public ICollection<Nurse> Nurses { get; set; } = new List<Nurse>();
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
