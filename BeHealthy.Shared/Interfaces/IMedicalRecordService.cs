using BeHealthy.Shared.Models.Dtos.MedicalRecord;

namespace BeHealthy.Shared.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync();
    Task<MedicalRecordDto> GetMedicalRecordByIdAsync(int id);
    Task AddMedicalRecordAsync(MedicalRecordForCreationDto medicalRecordDto);
    Task UpdateMedicalRecordAsync(MedicalRecordForUpdateDto medicalRecordDto);
    Task DeleteMedicalRecordAsync(int id);
}
