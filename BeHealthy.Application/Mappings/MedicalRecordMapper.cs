using BeHealthy.Application.Dtos.MedicalRecord;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class MedicalRecordMapper
{
    public static MedicalRecordDto MapToDto(this MedicalRecord medicalRecord)
    {
        return new MedicalRecordDto
        {
            Id = medicalRecord.Id,
            PatientId = medicalRecord.PatientId,
            RecordDate = medicalRecord.RecordDate,
            Notes = medicalRecord.Notes
        };
    }

    public static MedicalRecord MapToDomain(this MedicalRecordDto dto)
    {
        return new MedicalRecord
        {
            Id = dto.Id,
            PatientId = dto.PatientId,
            RecordDate = dto.RecordDate,
            Notes = dto.Notes
        };
    }

    public static MedicalRecord MapToDomain(this MedicalRecordCreateDto dto)
    {
        return new MedicalRecord
        {
            PatientId = dto.PatientId,
            RecordDate = dto.RecordDate,
            Notes = dto.Notes
        };
    }

    public static MedicalRecord MapToDomain(this MedicalRecordUpdateDto dto)
    {
        return new MedicalRecord
        {
            Id = dto.Id,
            RecordDate = dto.RecordDate,
            Notes = dto.Notes
        };
    }

    public static IEnumerable<MedicalRecordDto> MapToDto(this IEnumerable<MedicalRecord> medicalRecords)
    {
        return medicalRecords.Select(medicalRecord => medicalRecord.MapToDto()).ToList();
    }

    public static IEnumerable<MedicalRecord> MapToDomain(this IEnumerable<MedicalRecordDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain()).ToList();
    }
}
