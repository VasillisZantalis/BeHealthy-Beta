namespace BeHealthy.Shared.Dtos.MedicalRecord;

public class MedicalRecordUpdateRequest
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
    public string? CreatedBy { get; set; }
}
