using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.MedicalRecord;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;

    public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
    }

    public async Task<IEnumerable<MedicalRecordResponse>> GetAllMedicalRecordsAsync(CancellationToken cancellationToken = default)
    {
        var medicalRecords = await _medicalRecordRepository.GetAllAsync(cancellationToken);
        return medicalRecords.MapToDto();
    }

    public async Task<MedicalRecordResponse?> GetMedicalRecordByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id, cancellationToken);
        return medicalRecord?.MapToDto();
    }

    public async Task AddMedicalRecordAsync(MedicalRecordCreateRequest medicalRecordDto, CancellationToken cancellationToken = default)
    {
        var medicalRecord = medicalRecordDto.MapToDomain();
        await _medicalRecordRepository.AddAsync(medicalRecord, cancellationToken);
        await _medicalRecordRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ServiceResponse> UpdateMedicalRecordAsync(MedicalRecordUpdateRequest medicalRecordDto, CancellationToken cancellationToken = default)
    {
        var medicalRecord = await _medicalRecordRepository.GetByIdAsync(medicalRecordDto.Id, cancellationToken);
        if (medicalRecord is null)
            return ServiceResponse.Failed(Resource.NotFound);

        medicalRecord.PatientId = medicalRecordDto.PatientId;
        medicalRecord.RecordDate = medicalRecordDto.RecordDate;
        medicalRecord.Notes = medicalRecordDto.Notes;
        medicalRecord.CreatedBy = medicalRecordDto.CreatedBy;

        await _medicalRecordRepository.UpdateAsync(medicalRecord);
        await _medicalRecordRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task DeleteMedicalRecordAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _medicalRecordRepository.DeleteAsync(id, cancellationToken))
            await _medicalRecordRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<MedicalRecordResponse>> GetMedicalRecordsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var medicalRecords = await _medicalRecordRepository.GetMedicalRecordsByPatientIdAsync(patientId, cancellationToken);
        return medicalRecords.MapToDto();
    }

    public async Task UpdateMedicalRecordNotesAsync(int id, string notes, CancellationToken cancellationToken = default)
    {
        var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id, cancellationToken);
        if (medicalRecord is null)
            return;

        medicalRecord.Notes = notes;
        await _medicalRecordRepository.SaveChangesAsync(cancellationToken);
    }
}
