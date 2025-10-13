namespace BeHealthy.Domain.Entities;

public class Visit
{
    public int Id { get; set; }
    public DateTime VisitDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public int MedicalRecordId { get; set; }
    public MedicalRecord? MedicalRecord { get; set; }

    public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    public ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}
