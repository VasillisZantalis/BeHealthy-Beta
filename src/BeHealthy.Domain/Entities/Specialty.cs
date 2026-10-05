namespace BeHealthy.Domain.Entities;

public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
