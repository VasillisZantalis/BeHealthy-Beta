using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Models.Dtos.Appointment;

public class AppointmentForCreationDto
{
    [Required(ErrorMessage = "Patient is required")]
    public string? PatientId { get; set; }

    [Required(ErrorMessage = "Doctor is required")]
    public string? DoctorId { get; set; }

    [Required(ErrorMessage = "Appointment Date is required")]
    public DateTime AppointmentDate { get; set; }

    public string Notes { get; set; } = string.Empty;
}
