namespace BeHealthy.Shared.Dtos.Prescription;

public class PrescriptionCreateRequest
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }
}
