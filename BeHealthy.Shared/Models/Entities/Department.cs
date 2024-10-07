namespace BeHealthy.Shared.Models.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public ICollection<Nurse> Nurses { get; set; } = new List<Nurse>();
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
