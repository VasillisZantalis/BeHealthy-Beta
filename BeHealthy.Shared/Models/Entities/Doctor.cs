namespace BeHealthy.Shared.Models.Entities;

public class Doctor : BaseEntity
{
    public string Specialty { get; set; } = string.Empty;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

