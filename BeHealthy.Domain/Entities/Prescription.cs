namespace BeHealthy.Domain.Entities;

public class Prescription
{
    public int Id { get; set; }
    public string Medication { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public DateTime DatePrescribed { get; set; }

    public int TreatmentId { get; set; }
    public Treatment Treatment { get; set; } = new();

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
}
