namespace BeHealthy.Domain.Entities;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Number { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
}
