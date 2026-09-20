using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.MedicalRecord;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;

    public MedicalRecordService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync()
    {
        var medicalRecords = await _unitOfWork.MedicalRecordRepository.GetAllAsync();
        return medicalRecords.MapToDto();
    }

    public async Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id)
    {
        var medicalRecord = await _unitOfWork.MedicalRecordRepository.GetByIdAsync(id);
        return medicalRecord?.MapToDto();
    }

    public async Task AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _unitOfWork.MedicalRecordRepository.AddAsync(medicalRecord);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ServiceResponse> UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto)
    {
        var medicalRecord = await _unitOfWork.MedicalRecordRepository.GetByIdAsync(medicalRecordDto.Id);
        if (medicalRecord is null)
            return ServiceResponse.Failed(Resource.NotFound);

        medicalRecord.PatientId = medicalRecordDto.PatientId;
        medicalRecord.RecordDate = medicalRecordDto.RecordDate;
        medicalRecord.Notes = medicalRecordDto.Notes;
        medicalRecord.CreatedBy = medicalRecordDto.CreatedBy;

        await _unitOfWork.MedicalRecordRepository.UpdateAsync(medicalRecord);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task DeleteMedicalRecordAsync(int id)
    {
        await _unitOfWork.MedicalRecordRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId)
    {
        var medicalRecords = await _unitOfWork.MedicalRecordRepository.GetMedicalRecordsByPatientIdAsync(patientId);
        return medicalRecords.MapToDto();
    }

    public async Task UpdateMedicalRecordNotesAsync(int id, string notes)
    {
        await _unitOfWork.MedicalRecordRepository.UpdateMedicalRecordNotesAsync(id, notes);
    }
}
