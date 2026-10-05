namespace BeHealthy.Shared.Dtos.MedicalRecord;

public class MedicalRecordCreateRequest
{
    public int PatientId { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
    public string? CreatedBy { get; set; }
}
