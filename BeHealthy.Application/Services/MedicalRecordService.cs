using BeHealthy.Application.Dtos.MedicalRecord;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Application.Mappings;

namespace BeHealthy.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;

    public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
    }

    public async Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync()
    {
        var medicalRecords = await _medicalRecordRepository.GetAllAsync();
        return medicalRecords.MapToDto();
    }

    public async Task<MedicalRecordDto> GetMedicalRecordByIdAsync(int id)
    {
        var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
        return medicalRecord.MapToDto();
    }

    public async Task AddMedicalRecordAsync(MedicalRecordForCreationDto medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _medicalRecordRepository.AddAsync(medicalRecord);
    }

    public async Task UpdateMedicalRecordAsync(MedicalRecordForUpdateDto medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _medicalRecordRepository.UpdateAsync(medicalRecord);
    }

    public async Task DeleteMedicalRecordAsync(int id)
    {
        await _medicalRecordRepository.DeleteAsync(id);
    }
}
