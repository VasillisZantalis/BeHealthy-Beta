namespace BeHealthy.Shared.Models.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Notes { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }
}
