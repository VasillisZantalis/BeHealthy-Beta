using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync();
    Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id);
    Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId);
    Task AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto);
    Task UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto);
    Task DeleteMedicalRecordAsync(int id);
    Task UpdateMedicalRecordNotesAsync(int id, string notes);
}
