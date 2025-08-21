namespace BeHealthy.Domain.DTOs;

public class VisitDto
{
    public int Id { get; set; }
    public DateTime VisitDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int MedicalRecordId { get; set; }
}