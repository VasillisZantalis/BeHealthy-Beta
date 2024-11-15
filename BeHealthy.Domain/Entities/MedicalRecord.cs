namespace BeHealthy.Domain.Entities;

public class MedicalRecord
{
    public int Id { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime RecordDate { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = new();

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = new();
}
