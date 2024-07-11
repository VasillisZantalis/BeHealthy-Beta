using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;


    public string? PatientId { get; set; }
    public ApplicationUser? Patient { get; set; }
    public string? DoctorId { get; set; }
    public ApplicationUser? Doctor { get; set; }
}
