using BeHealthy.Domain.Entities;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Application.Mappings;

public static class VisitMapper
{
    public static Visit MapToDomain(this VisitCreateRequest dto)
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

    public static void MapToEntity(this VisitUpdateRequest dto, Visit entity)
    {
        entity.VisitDate = dto.VisitDate;
        entity.Reason = dto.Reason;
        entity.Notes = dto.Notes;
        entity.PatientId = dto.PatientId;
        entity.DoctorId = dto.DoctorId;
        entity.MedicalRecordId = dto.MedicalRecordId;
    }

    public static VisitResponse MapToDto(this Visit visit)
    {
        return new VisitResponse
        {
            Id = visit.Id,
            VisitDate = visit.VisitDate,
            Reason = visit.Reason,
            Notes = visit.Notes,
            MedicalRecordId = visit.MedicalRecordId,
            Patient = visit.Patient != null ? visit.Patient.MapToSimpleDto() : new PatientSimpleResponse(),
            Doctor = visit.Doctor != null ? visit.Doctor.MapToSimpleDto() : new DoctorSimpleResponse(),
        };
    }

    public static IEnumerable<VisitResponse> MapToDto(this IEnumerable<Visit> visits) 
        => visits.Select(visit => visit.MapToDto());

    public static VisitCreateRequest MapToCreateDto(this VisitResponse visit)
    {
        return new VisitCreateRequest
        {
            VisitDate = visit.VisitDate,
            Reason = visit.Reason,
            Notes = visit.Notes,
            PatientId = visit.Patient.Id,
            DoctorId = visit.Doctor.Id,
            MedicalRecordId = visit.MedicalRecordId
        };
    }

    public static VisitUpdateRequest MapToUpdateDto(this VisitResponse visit)
    {
        return new VisitUpdateRequest
        {
            Id = visit.Id,
            VisitDate = visit.VisitDate,
            Reason = visit.Reason,
            Notes = visit.Notes,
            PatientId = visit.Patient.Id,
            DoctorId = visit.Doctor.Id,
            MedicalRecordId = visit.MedicalRecordId
        };
    }
}