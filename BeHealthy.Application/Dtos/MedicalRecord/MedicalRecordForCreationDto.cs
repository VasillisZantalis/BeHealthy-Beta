namespace BeHealthy.Application.Dtos.MedicalRecord;

public class MedicalRecordForCreationDto
{
    public int PatientId { get; set; }

    public string? Notes { get; set; }
    public DateTime RecordDate { get; set; }
}
