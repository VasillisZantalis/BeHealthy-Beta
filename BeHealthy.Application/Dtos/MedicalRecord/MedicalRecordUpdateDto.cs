namespace BeHealthy.Application.Dtos.MedicalRecord;

public class MedicalRecordUpdateDto
{
    public int Id { get; set; }
    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
}
