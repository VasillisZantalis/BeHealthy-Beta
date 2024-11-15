namespace BeHealthy.Application.Dtos.Prescription;

public class PrescriptionForCreationDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }
}
