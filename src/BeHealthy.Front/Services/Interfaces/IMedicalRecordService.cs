using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.MedicalRecord;

namespace BeHealthy.Front.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync();
    Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id);
    Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId);
    Task<ServiceResponse> AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto);
    Task<ServiceResponse> UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto);
    Task DeleteMedicalRecordAsync(int id);
    Task<ServiceResponse> UpdateMedicalRecordNotesAsync(int id, MedicalRecordNotesUpdateRequest notesDto);
}
