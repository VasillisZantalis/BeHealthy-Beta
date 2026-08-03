using BeHealthy.Application.Mappings;
using BeHealthy.Domain.Entities;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.API.Mapping;

public static class VisitMappingExtensions
{
    public static DiagnosisResponse MapToDto(this Diagnosis diagnosis) => new()
    {
        Id = diagnosis.Id,
        Name = diagnosis.Name,
        Notes = diagnosis.Notes,
        VisitId = diagnosis.VisitId
    };

    public static TreatmentResponse MapToDto(this Treatment treatment) => new()
    {
        Id = treatment.Id,
        Description = treatment.Description,
        StartDate = treatment.StartDate,
        EndDate = treatment.EndDate,
        VisitId = treatment.VisitId,
        DiagnosisId = treatment.DiagnosisId
    };

    public static LabResultResponse MapToDto(this LabResult labResult) => new()
    {
        Id = labResult.Id,
        TestName = labResult.TestName,
        ResultValue = labResult.ResultValue,
        Unit = labResult.Unit,
        ReferenceRange = labResult.ReferenceRange,
        ResultDate = labResult.ResultDate,
        VisitId = labResult.VisitId
    };

    public static VisitDetailsResponse MapToDetailsDto(this Visit visit) => new()
    {
        Id = visit.Id,
        VisitDate = visit.VisitDate,
        Reason = visit.Reason,
        Notes = visit.Notes,
        Doctor = visit.Doctor?.MapToSimpleDto() ?? new(),
        Patient = visit.Patient?.MapToSimpleDto() ?? new(),
        MedicalRecordId = visit.MedicalRecordId,
        Diagnoses = [.. visit.Diagnoses.Select(d => d.MapToDto())],
        Treatments = [.. visit.Treatments.Select(t => t.MapToDto())],
        LabResults = [.. visit.LabResults.Select(l => l.MapToDto())]
    };
}
