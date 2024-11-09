using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentForCreationDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentReason Reason { get; set; }
    public AppointmentStatus Status { get; set; }

    public string Notes { get; set; } = string.Empty;
    public int Duration { get; set; }
}
