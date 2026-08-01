namespace BeHealthy.Shared.Dtos.Visit;

public class VisitCreateDto
{
    public DateTime VisitDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int MedicalRecordId { get; set; }
}