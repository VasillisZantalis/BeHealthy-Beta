namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentForUpdateDto
{
    public int Id { get; set; }
    public string? PatientId { get; set; }
    public string? DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}
