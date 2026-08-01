using BeHealthy.Shared.Dtos.MedicalRecord;

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

    public async Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(int id)
    {
        var medicalRecord = await _unitOfWork.MedicalRecordRepository.GetByIdAsync(id);
        return medicalRecord?.MapToDto();
    }

    public async Task AddMedicalRecordAsync(MedicalRecordCreateDto medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _unitOfWork.MedicalRecordRepository.AddAsync(medicalRecord);
    }

    public async Task UpdateMedicalRecordAsync(MedicalRecordUpdateDto medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _unitOfWork.MedicalRecordRepository.UpdateAsync(medicalRecord);
    }

    public async Task DeleteMedicalRecordAsync(int id)
    {
        await _unitOfWork.MedicalRecordRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientIdAsync(int patientId)
    {
        var medicalRecords = await _unitOfWork.MedicalRecordRepository.GetMedicalRecordsByPatientIdAsync(patientId);
        return medicalRecords.MapToDto();
    }

    public async Task UpdateMedicalRecordNotesAsync(int id, string notes)
    {
        await _unitOfWork.MedicalRecordRepository.UpdateMedicalRecordNotesAsync(id, notes);
    }
}
