using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class PrescriptionMapper
{
    public static PrescriptionResponse MapToDto(this Prescription prescription)
    {
        return new PrescriptionResponse
        {
            Id = prescription.Id,
            PatientId = prescription.PatientId,
            Patient = prescription.Patient != null ? prescription.Patient.MapToSimpleDto() : new PatientSimpleResponse(),
            DoctorId = prescription.DoctorId,
            Doctor = prescription.Doctor != null ? prescription.Doctor.MapToSimpleDto() : new DoctorSimpleResponse(),
            Medication = prescription.Medication,
            Dosage = prescription.Dosage,
            DatePrescribed = prescription.DatePrescribed
        };
    }

    public static Prescription MapToDomain(this PrescriptionResponse dto)
    {
        return new Prescription
        {
            Id = dto.Id,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Medication = dto.Medication,
            Dosage = dto.Dosage,
            DatePrescribed = dto.DatePrescribed
        };
    }

    public static Prescription MapToDomain(this PrescriptionCreateRequest dto)
    {
        return new Prescription
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            Medication = dto.Medication,
            Dosage = dto.Dosage,
            DatePrescribed = dto.DatePrescribed
        };
    }

    public static Prescription MapToDomain(this PrescriptionUpdateRequest dto)
    {
        return new Prescription
        {
            Id = dto.Id,
            Medication = dto.Medication,
            Dosage = dto.Dosage
        };
    }

    public static PrescriptionUpdateRequest MapToUpdateDto(this Prescription prescription)
    {
        return new PrescriptionUpdateRequest
        {
            Id = prescription.Id,
            Medication = prescription.Medication,
            Dosage = prescription.Dosage
        };
    }

    public static PrescriptionCreateRequest MapDtoToCreateDto(this PrescriptionResponse prescription)
    {
        return new PrescriptionCreateRequest
        {
            Medication = prescription.Medication,
            Dosage = prescription.Dosage,
            DatePrescribed = prescription.DatePrescribed,
            DoctorId = prescription.DoctorId,
            PatientId = prescription.PatientId
        };
    }

    public static PrescriptionUpdateRequest MapDtoToUpdateDto(this PrescriptionResponse prescription)
    {
        return new PrescriptionUpdateRequest
        {
            Id = prescription.Id,
            Medication = prescription.Medication,
            Dosage = prescription.Dosage
        };
    }


    public static IEnumerable<PrescriptionResponse> MapToDto(this IEnumerable<Prescription> prescriptions)
    {
        return prescriptions.Select(p => p.MapToDto());
    }

    public static IEnumerable<Prescription> MapToDomain(this IEnumerable<PrescriptionResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }
}
