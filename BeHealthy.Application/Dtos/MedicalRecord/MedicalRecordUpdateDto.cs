namespace BeHealthy.Application.Dtos.MedicalRecord;

public class MedicalRecordUpdateDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
    public string? CreatedBy { get; set; }
}
