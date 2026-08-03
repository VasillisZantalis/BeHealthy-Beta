using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;

namespace BeHealthy.Shared.Dtos.Visit;

public class VisitDetailsResponse
{
    public int Id { get; set; }
    public DateTime VisitDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DoctorSimpleResponse Doctor { get; set; } = new();
    public PatientSimpleResponse Patient { get; set; } = new();
    public int MedicalRecordId { get; set; }
    public List<DiagnosisResponse> Diagnoses { get; set; } = [];
    public List<TreatmentResponse> Treatments { get; set; } = [];
    public List<LabResultResponse> LabResults { get; set; } = [];
}
