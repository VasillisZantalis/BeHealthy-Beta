using BeHealthy.Shared.Models.Dtos.Doctor;
using BeHealthy.Shared.Models.Dtos.Patient;

namespace BeHealthy.Shared.Models.Dtos.Prescription;

public class PrescriptionDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public PatientSimpleDto Patient { get; set; } = new();
    public int DoctorId { get; set; }
    public DoctorSimpleDto Doctor { get; set; } = new();
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }
}
