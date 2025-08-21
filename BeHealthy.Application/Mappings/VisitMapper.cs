using BeHealthy.Domain.Entities;
using BeHealthy.Application.Dtos.Visit;

namespace BeHealthy.Application.Mappings;

public static class VisitMapper
{
    public static Visit MapToDomain(this VisitCreateDto dto)
    {
        return new Visit
        {
            VisitDate = dto.VisitDate,
            Reason = dto.Reason,
            Notes = dto.Notes,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            MedicalRecordId = dto.MedicalRecordId
        };
    }

    public static void MapToEntity(this VisitUpdateDto dto, Visit entity)
    {
        entity.VisitDate = dto.VisitDate;
        entity.Reason = dto.Reason;
        entity.Notes = dto.Notes;
        entity.PatientId = dto.PatientId;
        entity.DoctorId = dto.DoctorId;
        entity.MedicalRecordId = dto.MedicalRecordId;
    }

    public static VisitDto MapToDto(this Visit visit)
    {
        return new VisitDto
        {
            Id = visit.Id,
            VisitDate = visit.VisitDate,
            Reason = visit.Reason,
            Notes = visit.Notes,
            MedicalRecordId = visit.MedicalRecordId,
            Patient = visit.Patient != null ? visit.Patient.MapToSimpleDto() : new PatientSimpleDto(),
            Doctor = visit.Doctor != null ? visit.Doctor.MapToSimpleDto() : new DoctorSimpleDto(),
        };
    }

    public static IEnumerable<VisitDto> MapToDto(this IEnumerable<Visit> visits) 
        => visits.Select(visit => visit.MapToDto());
}