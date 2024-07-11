namespace BeHealthy.Shared.Models.Dtos.MedicalRecord;

public class MedicalRecordForUpdateDto
{
    public int Id { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime RecordDate { get; set; }
}
