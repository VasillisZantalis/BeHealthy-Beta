using BeHealthy.Shared.Dtos.Appointment;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings
{
    public static class AppointmentMapper
    {
        public static Appointment MapToDomain(this AppointmentResponse dto)
        {
            return new Appointment
            {
                Id = dto.Id,
                AppointmentDate = dto.AppointmentDate,
                AppointmentStartTime = dto.AppointmentStartTime,
                AppointmentEndTime = dto.AppointmentEndTime,
                Notes = dto.Notes,
                Status = dto.Status,
                Reason = dto.Reason,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                RoomId = dto.RoomId,
                NurseId = dto.NurseId
            };
        }

        public static AppointmentResponse MapToDto(this Appointment entity)
        {
            return new AppointmentResponse
            {
                Id = entity.Id,
                AppointmentDate = entity.AppointmentDate,
                AppointmentStartTime = entity.AppointmentStartTime,
                AppointmentEndTime = entity.AppointmentEndTime,
                Notes = entity.Notes ?? string.Empty,
                Status = entity.Status,
                Reason = entity.Reason,
                PatientId = entity.PatientId,
                Patient = entity.Patient?.MapToDto(),
                DoctorId = entity.DoctorId,
                Doctor = entity.Doctor?.MapToDto(),
                RoomId = entity.RoomId,
                Room = entity.Room?.MapToDto(),
                NurseId = entity.NurseId,
                Nurse = entity.Nurse?.MapToDto(),
            };
        }

        public static Appointment MapToDomain(this AppointmentCreateRequest dto)
        {
            return new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentStartTime = dto.AppointmentStartTime,
                AppointmentEndTime = dto.AppointmentEndTime,
                Notes = dto.Notes,
                Status = dto.Status,
                Reason = dto.Reason,
                RoomId = dto.RoomId,
                NurseId = dto.NurseId
            };
        }

        public static Appointment MapToDomain(this AppointmentUpdateRequest dto)
        {
            return new Appointment
            {
                Id = dto.Id,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentStartTime = dto.AppointmentStartTime,
                AppointmentEndTime = dto.AppointmentEndTime,
                Notes = dto.Notes,
                Status = dto.Status,
                Reason = dto.Reason,
                RoomId = dto.RoomId,
                NurseId = dto.NurseId
            };
        }

        public static AppointmentCreateRequest MapToCreationDto(this AppointmentResponse dto)
        {
            return new AppointmentCreateRequest
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
        }

        public static AppointmentUpdateRequest MapToUpdateDto(this AppointmentResponse dto)
        {
            return new AppointmentUpdateRequest
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
        }

        public static IEnumerable<Appointment> MapToDomain(this IEnumerable<AppointmentResponse> dtos) 
            => dtos.Select(dto => dto.MapToDomain());

        public static IEnumerable<AppointmentResponse> MapToDto(this IEnumerable<Appointment> entities)
            => entities.Select(entity => entity.MapToDto());
    }
}
