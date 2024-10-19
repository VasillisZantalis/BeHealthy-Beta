using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Patient;

namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    public AppointmentReason Reason { get; set; }

    public int? PatientId { get; set; }
    public PatientDto? Patient { get; set; }
    public int? DoctorId { get; set; }
    public DoctorDto? Doctor { get; set; }
}
