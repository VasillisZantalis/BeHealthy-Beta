using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Application.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync();
    Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(int id);
    Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientIdAsync(int patientId);
    Task AddMedicalRecordAsync(MedicalRecordCreateDto medicalRecordDto);
    Task UpdateMedicalRecordAsync(MedicalRecordUpdateDto medicalRecordDto);
    Task DeleteMedicalRecordAsync(int id);
    Task UpdateMedicalRecordNotesAsync(int id, string notes);
}
