using BeHealthy.Domain;

namespace BeHealthy.Application.Dtos.Appointment;

public class AppointmentForCreationDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int RoomId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentReason Reason { get; set; }
    public AppointmentStatus Status { get; set; }

    public string Notes { get; set; } = string.Empty;
    public int Duration { get; set; }
}
