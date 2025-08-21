namespace BeHealthy.Application.Dtos.Visit;

public class VisitDto
{
    public int Id { get; set; }
    public DateTime VisitDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DoctorSimpleDto Doctor { get; set; } = new();
    public PatientSimpleDto Patient { get; set; } = new();
    public int MedicalRecordId { get; set; }
}