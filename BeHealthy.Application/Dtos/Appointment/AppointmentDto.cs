using BeHealthy.Application.Dtos.Doctor;
using BeHealthy.Application.Dtos.Patient;
using BeHealthy.Domain;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }
    [Required(ErrorMessage = "Reason is required")]
    public AppointmentReason Reason { get; set; }
    [Range(1, 1440, ErrorMessage = "Duration must be between 1-1440")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Patient is required")]
    public int PatientId { get; set; }
    public PatientDto? Patient { get; set; }
    [Required(ErrorMessage = "Doctor is required")]
    public int DoctorId { get; set; }
    public DoctorDto? Doctor { get; set; }
}
