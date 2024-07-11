namespace BeHealthy.Shared.Models.Entities;

public class Patient : BaseEntity
{
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

