using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Dtos.Prescription;

public class PrescriptionResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public PatientSimpleResponse Patient { get; set; } = new();
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Doctor is required")]
    public int DoctorId { get; set; }
    public DoctorSimpleResponse Doctor { get; set; } = new();
    [Required]
    public string Medication { get; set; } = string.Empty;
    [Required]
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }
}
