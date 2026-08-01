using BeHealthy.Shared.Dtos.Allergy;
using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Shared.Dtos.Department;
using BeHealthy.Shared.Dtos.Doctor;
using BeHealthy.Shared.Dtos.MedicalRecord;
using BeHealthy.Shared.Dtos.Nurse;
using BeHealthy.Shared.Dtos.Patient;
using BeHealthy.Shared.Dtos.Prescription;
using BeHealthy.Shared.Dtos.Room;
using BeHealthy.Shared.Dtos.Visit;

namespace BeHealthy.Frontend.Mappings;

/// <summary>
/// DTO-to-DTO mapping helpers used by edit/create forms. These were extracted from the
/// server-side BeHealthy.Application mappers, keeping only the pure DTO transformations
/// (all Domain-entity mappings stay on the server / API).
/// </summary>
public static class DtoMappers
{
    // ---- Allergy ----
    public static AllergyCreateDto MapToCreateDto(this AllergyDto dto) => new()
    {
        AllergyName = dto.AllergyName,
        Allergen = dto.Allergen,
        Severity = dto.Severity,
        Notes = dto.Notes,
        PatientId = dto.PatientId
    };

    public static AllergyUpdateDto MapToUpdateDto(this AllergyDto dto) => new()
    {
        Id = dto.Id,
        AllergyName = dto.AllergyName,
        Allergen = dto.Allergen,
        Severity = dto.Severity,
        Notes = dto.Notes,
        PatientId = dto.PatientId
    };

    // ---- Appointment ----
    public static AppointmentCreateDto MapToCreationDto(this AppointmentDto dto) => new()
    {
        DoctorId = dto.DoctorId,
        PatientId = dto.PatientId,
        Notes = dto.Notes,
        AppointmentDate = dto.AppointmentDate,
        AppointmentStartTime = dto.AppointmentStartTime,
        AppointmentEndTime = dto.AppointmentEndTime,
        Reason = dto.Reason,
        Status = dto.Status,
        RoomId = dto.RoomId,
        NurseId = dto.NurseId
    };

    public static AppointmentUpdateDto MapToUpdateDto(this AppointmentDto dto) => new()
    {
        Id = dto.Id,
        DoctorId = dto.DoctorId,
        PatientId = dto.PatientId,
        Notes = dto.Notes,
        AppointmentDate = dto.AppointmentDate,
        AppointmentStartTime = dto.AppointmentStartTime,
        AppointmentEndTime = dto.AppointmentEndTime,
        Reason = dto.Reason,
        Status = dto.Status,
        RoomId = dto.RoomId,
        NurseId = dto.NurseId
    };

    // ---- Department ----
    public static DepartmentCreateDto MapToCreationDto(this DepartmentDto department) => new()
    {
        Name = department.Name,
        Location = department.Location,
        HeadOfDepartmentId = department.HeadOfDepartmentId,
        Doctors = department.Doctors,
        Nurses = department.Nurses,
        Patients = department.Patients,
        Rooms = department.Rooms
    };

    public static DepartmentUpdateDto MapToUpdateDto(this DepartmentDto department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        Location = department.Location,
        HeadOfDepartmentId = department.HeadOfDepartmentId,
        Doctors = department.Doctors,
        Nurses = department.Nurses,
        Patients = department.Patients,
        Rooms = department.Rooms
    };

    // ---- Doctor ----
    public static DoctorUpdateDto MapToUpdateDto(this DoctorDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Image = dto.Image,
        UserId = dto.UserId,
        SpecialtyId = dto.SpecialtyId,
        PhoneNumber = dto.PhoneNumber,
        DepartmentId = dto.DepartmentId
    };

    public static DoctorDto MapToDto(this DoctorUpdateDto dto) => new()
    {
        Id = dto.Id,
        UserId = dto.UserId,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Image = dto.Image,
        SpecialtyId = dto.SpecialtyId,
        PhoneNumber = dto.PhoneNumber,
        CreatedAt = DateTime.UtcNow,
        DepartmentId = dto.DepartmentId
    };

    // ---- MedicalRecord ----
    public static MedicalRecordUpdateDto MapToMedicalRecordUpdateDto(this MedicalRecordDto medicalRecord) => new()
    {
        Id = medicalRecord.Id,
        PatientId = medicalRecord.PatientId,
        RecordDate = medicalRecord.RecordDate,
        Notes = medicalRecord.Notes,
        CreatedBy = medicalRecord.CreatedBy
    };

    public static MedicalRecordCreateDto MapToMedicalRecordCreateDto(this MedicalRecordDto medicalRecord) => new()
    {
        PatientId = medicalRecord.PatientId,
        RecordDate = medicalRecord.RecordDate,
        Notes = medicalRecord.Notes,
        CreatedBy = medicalRecord.CreatedBy
    };

    // ---- Nurse ----
    public static NurseUpdateDto MapToUpdateDto(this NurseDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Image = dto.Image,
        UserId = dto.UserId ?? string.Empty,
        PhoneNumber = dto.PhoneNumber,
        DepartmentId = dto.DepartmentId
    };

    // ---- Patient ----
    public static PatientUpdateDto MapToUpdateDto(this PatientDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Image = dto.Image,
        UserId = dto.UserId ?? string.Empty,
        PhoneNumber = dto.PhoneNumber,
        DepartmentId = dto.DepartmentId
    };

    public static PatientUpdateDto MapToDtoForUpdate(this PatientDto dto) => dto.MapToUpdateDto();

    public static PatientDto MapToDto(this PatientUpdateDto dto) => new()
    {
        Id = dto.Id,
        UserId = dto.UserId,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Image = dto.Image,
        PhoneNumber = dto.PhoneNumber,
        CreatedAt = DateTime.UtcNow,
        DepartmentId = dto.DepartmentId
    };

    // ---- Prescription ----
    public static PrescriptionCreateDto MapDtoToCreateDto(this PrescriptionDto prescription) => new()
    {
        Medication = prescription.Medication,
        Dosage = prescription.Dosage,
        DatePrescribed = prescription.DatePrescribed,
        DoctorId = prescription.DoctorId,
        PatientId = prescription.PatientId
    };

    public static PrescriptionUpdateDto MapDtoToUpdateDto(this PrescriptionDto prescription) => new()
    {
        Id = prescription.Id,
        Medication = prescription.Medication,
        Dosage = prescription.Dosage
    };

    // ---- Room ----
    public static RoomDto MapToSelf(this RoomDto room) => new()
    {
        Id = room.Id,
        Name = room.Name,
        Number = room.Number,
        Department = room.Department
    };

    public static RoomUpdateDto MapDtoToUpdateDto(this RoomDto room) => new()
    {
        Id = room.Id,
        Name = room.Name,
        Number = room.Number,
        DepartmentId = room.DepartmentId
    };

    public static RoomCreateDto MapDtoToCreateDto(this RoomDto room) => new()
    {
        Name = room.Name,
        Number = room.Number,
        DepartmentId = room.DepartmentId
    };

    // ---- Visit ----
    public static VisitCreateDto MapToCreateDto(this VisitDto visit) => new()
    {
        VisitDate = visit.VisitDate,
        Reason = visit.Reason,
        Notes = visit.Notes,
        PatientId = visit.Patient.Id,
        DoctorId = visit.Doctor.Id,
        MedicalRecordId = visit.MedicalRecordId
    };

    public static VisitUpdateDto MapToUpdateDto(this VisitDto visit) => new()
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
