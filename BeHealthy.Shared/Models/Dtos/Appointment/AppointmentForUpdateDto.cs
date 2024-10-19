namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentForUpdateDto
{
    public int Id { get; set; }
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public AppointmentReason Reason { get; set; }
}
