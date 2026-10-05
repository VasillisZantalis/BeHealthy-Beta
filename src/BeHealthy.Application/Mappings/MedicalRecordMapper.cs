using BeHealthy.Shared.Dtos.MedicalRecord;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class MedicalRecordMapper
{
    public static MedicalRecordResponse MapToDto(this MedicalRecord medicalRecord)
    {
        return new MedicalRecordResponse
        {
            Id = medicalRecord.Id,
            PatientId = medicalRecord.PatientId,
            RecordDate = medicalRecord.RecordDate,
            Notes = medicalRecord.Notes,
            CreatedBy = medicalRecord.CreatedBy
        };
    }

    public static MedicalRecord MapToDomain(this MedicalRecordResponse dto)
    {
        return new MedicalRecord
        {
            Id = dto.Id,
            PatientId = dto.PatientId,
            RecordDate = dto.RecordDate,
            Notes = dto.Notes,
            CreatedBy = dto.CreatedBy
        };
    }

    public static MedicalRecord MapToDomain(this MedicalRecordCreateRequest dto)
    {
        return new MedicalRecord
        {
            PatientId = dto.PatientId,
            RecordDate = dto.RecordDate,
            Notes = dto.Notes,
            CreatedBy = dto.CreatedBy,
        };
    }

    public static MedicalRecord MapToDomain(this MedicalRecordUpdateRequest dto)
    {
        return new MedicalRecord
        {
            Id = dto.Id,
            PatientId = dto.PatientId,
            RecordDate = dto.RecordDate,
            Notes = dto.Notes,
            CreatedBy = dto.CreatedBy,
        };
    }

    public static MedicalRecordUpdateRequest MapToMedicalRecordUpdateDto(this MedicalRecordResponse medicalRecord)
    {
        return new MedicalRecordUpdateRequest
        {
            Id = medicalRecord.Id,
            PatientId = medicalRecord.PatientId,
            RecordDate = medicalRecord.RecordDate,
            Notes = medicalRecord.Notes,
            CreatedBy = medicalRecord.CreatedBy
        };
    }

    public static MedicalRecordCreateRequest MapToMedicalRecordCreateDto(this MedicalRecordResponse medicalRecord)
    {
        return new MedicalRecordCreateRequest
        {
            PatientId = medicalRecord.PatientId,
            RecordDate = medicalRecord.RecordDate,
            Notes = medicalRecord.Notes,
            CreatedBy = medicalRecord.CreatedBy
        };
    }

    public static IEnumerable<MedicalRecordResponse> MapToDto(this IEnumerable<MedicalRecord> medicalRecords)
    {
        return medicalRecords.Select(medicalRecord => medicalRecord.MapToDto()).ToList();
    }

    public static IEnumerable<MedicalRecord> MapToDomain(this IEnumerable<MedicalRecordResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain()).ToList();
    }
}
