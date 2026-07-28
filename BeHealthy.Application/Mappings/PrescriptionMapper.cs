using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class PrescriptionMapper
{
    public static PrescriptionDto MapToDto(this Prescription prescription)
    {
        return new PrescriptionDto
        {
            Id = prescription.Id,
            PatientId = prescription.PatientId,
            Patient = prescription.Patient != null ? prescription.Patient.MapToSimpleDto() : new PatientSimpleDto(),
            DoctorId = prescription.DoctorId,
            Doctor = prescription.Doctor != null ? prescription.Doctor.MapToSimpleDto() : new DoctorSimpleDto(),
            Medication = prescription.Medication,
            Dosage = prescription.Dosage,
            DatePrescribed = prescription.DatePrescribed
        };
    }

    public static Prescription MapToDomain(this PrescriptionDto dto)
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

    public static Prescription MapToDomain(this PrescriptionCreateDto dto)
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

    public static Prescription MapToDomain(this PrescriptionUpdateDto dto)
    {
        return new Prescription
        {
            Id = dto.Id,
            Medication = dto.Medication,
            Dosage = dto.Dosage
        };
    }

    public static PrescriptionUpdateDto MapToUpdateDto(this Prescription prescription)
    {
        return new PrescriptionUpdateDto
        {
            Id = prescription.Id,
            Medication = prescription.Medication,
            Dosage = prescription.Dosage
        };
    }

    public static PrescriptionCreateDto MapDtoToCreateDto(this PrescriptionDto prescription)
    {
        return new PrescriptionCreateDto
        {
            Medication = prescription.Medication,
            Dosage = prescription.Dosage,
            DatePrescribed = prescription.DatePrescribed,
            DoctorId = prescription.DoctorId,
            PatientId = prescription.PatientId
        };
    }

    public static PrescriptionUpdateDto MapDtoToUpdateDto(this PrescriptionDto prescription)
    {
        return new PrescriptionUpdateDto
        {
            Id = prescription.Id,
            Medication = prescription.Medication,
            Dosage = prescription.Dosage
        };
    }


    public static IEnumerable<PrescriptionDto> MapToDto(this IEnumerable<Prescription> prescriptions)
    {
        return prescriptions.Select(p => p.MapToDto());
    }

    public static IEnumerable<Prescription> MapToDomain(this IEnumerable<PrescriptionDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }
}
