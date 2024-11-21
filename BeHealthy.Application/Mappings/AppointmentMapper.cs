using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings
{
    public static class AppointmentMapper
    {
        public static Appointment MapToDomain(this AppointmentDto dto)
        {
            return new Appointment
            {
                Id = dto.Id,
                AppointmentDate = dto.AppointmentDate,
                Notes = dto.Notes,
                Status = dto.Status,
                Reason = dto.Reason,
                Duration = dto.Duration,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId
            };
        }

        public static AppointmentDto MapToDto(this Appointment entity)
        {
            return new AppointmentDto
            {
                Id = entity.Id,
                AppointmentDate = entity.AppointmentDate,
                Notes = entity.Notes ?? string.Empty,
                Status = entity.Status,
                Reason = entity.Reason,
                Duration = entity.Duration,
                PatientId = entity.PatientId,
                Patient = entity.Patient?.MapToDto(),
                DoctorId = entity.DoctorId,
                Doctor = entity.Doctor?.MapToDto()
            };
        }

        // Map from AppointmentForCreationDto to Appointment (MapToDomain)
        public static Appointment MapToDomain(this AppointmentForCreationDto dto)
        {
            return new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Notes = dto.Notes,
                Status = dto.Status,
                Reason = dto.Reason,
                Duration = dto.Duration
            };
        }

        public static Appointment MapToDomain(this AppointmentForUpdateDto dto)
        {
            return new Appointment
            {
                Id = dto.Id,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Notes = dto.Notes,
                Status = dto.Status,
                Reason = dto.Reason,
                Duration = dto.Duration
            };
        }

        public static IEnumerable<Appointment> MapToDomain(this IEnumerable<AppointmentDto> dtos)
        {
            return dtos.Select(dto => dto.MapToDomain());
        }

        public static IEnumerable<AppointmentDto> MapToDto(this IEnumerable<Appointment> entities)
        {
            return entities.Select(entity => entity.MapToDto());
        }
    }
}
