namespace BeHealthy.Shared.Models.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;


    // Patient relationship
    public string? PatientId { get; set; }
    public virtual ApplicationUser? Patient { get; set; }

    // Doctor relationship
    public string? DoctorId { get; set; }
    public virtual ApplicationUser? Doctor { get; set; }
}
