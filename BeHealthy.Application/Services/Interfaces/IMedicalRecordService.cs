using BeHealthy.Application.Dtos.MedicalRecord;

namespace BeHealthy.Application.Services.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync();
    Task<MedicalRecordDto> GetMedicalRecordByIdAsync(int id);
    Task AddMedicalRecordAsync(MedicalRecordForCreationDto medicalRecordDto);
    Task UpdateMedicalRecordAsync(MedicalRecordForUpdateDto medicalRecordDto);
    Task DeleteMedicalRecordAsync(int id);
}
