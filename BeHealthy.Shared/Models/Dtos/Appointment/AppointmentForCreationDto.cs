using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentForCreationDto
{
    [Required(ErrorMessage = "Patient is required")]
    public int? PatientId { get; set; }

    [Required(ErrorMessage = "Doctor is required")]
    public int? DoctorId { get; set; }

    [Required(ErrorMessage = "Appointment Date is required")]
    public DateTime AppointmentDate { get; set; }
    [Required(ErrorMessage = "Appointment Reason is required")]
    public AppointmentReason Reason { get; set; }
    public AppointmentStatus Status { get; set; }

    public string Notes { get; set; } = string.Empty;
}
