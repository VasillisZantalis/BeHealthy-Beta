using AutoMapper;
using BeHealthy.Application.Dtos.MedicalRecord;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces.Repositories;
using BeHealthy.Domain.Entities;
using BeHealthy.Domain.Interfaces.Repositories;

namespace BeHealthy.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IMapper _mapper;

    public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, IMapper mapper)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync()
    {
        var medicalRecords = await _medicalRecordRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MedicalRecordDto>>(medicalRecords);
    }

    public async Task<MedicalRecordDto> GetMedicalRecordByIdAsync(int id)
    {
        var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
        return _mapper.Map<MedicalRecordDto>(medicalRecord);
    }

    public async Task AddMedicalRecordAsync(MedicalRecordForCreationDto medicalRecordDto)
    {
        var medicalRecord = _mapper.Map<MedicalRecord>(medicalRecordDto);
        await _medicalRecordRepository.AddAsync(medicalRecord);
    }

    public async Task UpdateMedicalRecordAsync(MedicalRecordForUpdateDto medicalRecordDto)
    {
        var medicalRecord = _mapper.Map<MedicalRecord>(medicalRecordDto);
        await _medicalRecordRepository.UpdateAsync(medicalRecord);
    }

    public async Task DeleteMedicalRecordAsync(int id)
    {
        await _medicalRecordRepository.DeleteAsync(id);
    }
}
