using BeHealthy.Application.Dtos.MedicalRecord;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Application.Mappings;
using BeHealthy.Domain.Interfaces;
using BeHealthy.Infrastructure.Repositories;

namespace BeHealthy.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;

    public MedicalRecordService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync()
    {
        var medicalRecords = await _unitOfWork.MedicalRecordRepository.GetAllAsync();
        return medicalRecords.MapToDto();
    }

    public async Task<MedicalRecordDto> GetMedicalRecordByIdAsync(int id)
    {
        var medicalRecord = await _unitOfWork.MedicalRecordRepository.GetByIdAsync(id);
        return medicalRecord.MapToDto();
    }

    public async Task AddMedicalRecordAsync(MedicalRecordForCreationDto medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _unitOfWork.MedicalRecordRepository.AddAsync(medicalRecord);
    }

    public async Task UpdateMedicalRecordAsync(MedicalRecordForUpdateDto medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _unitOfWork.MedicalRecordRepository.UpdateAsync(medicalRecord);
    }

    public async Task DeleteMedicalRecordAsync(int id)
    {
        await _unitOfWork.MedicalRecordRepository.DeleteAsync(id);
    }
}
