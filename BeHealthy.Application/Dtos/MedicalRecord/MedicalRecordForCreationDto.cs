namespace BeHealthy.Application.Dtos.MedicalRecord;

public class MedicalRecordForCreationDto
{
    public int PatientId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime RecordDate { get; set; }
}
