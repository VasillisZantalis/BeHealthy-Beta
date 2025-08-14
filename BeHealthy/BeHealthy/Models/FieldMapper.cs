using BeHealthy.Application.Dtos.Appointment;
using BeHealthy.Domain;
using BeHealthy.Extensions;
using BeHealthy.Models.Enums;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Models;

public static class FieldMapper
{
    public static List<FieldDefinition> GetFields(
        ImportEntity entity,
        List<SelectItem> doctors,
        List<SelectItem> departments,
        List<SelectItem> patients)
    {
        switch (entity)
        {
            case ImportEntity.Doctor:
            case ImportEntity.Patient:
            case ImportEntity.Nurse:
                return new List<FieldDefinition>
                {
                    new FieldDefinition { Name = "FirstName", DisplayName = Resource.FirstName, Type = FieldType.Text, IsRequired = true },
                    new FieldDefinition { Name = "LastName", DisplayName = Resource.LastName, Type = FieldType.Text, IsRequired = true },
                    new FieldDefinition { Name = "Email", DisplayName = Resource.Email, Type = FieldType.Text, IsRequired = true },
                    new FieldDefinition { Name = "Password", DisplayName = Resource.Password, Type = FieldType.Password, IsRequired = true },
                    new FieldDefinition { Name = "Department", DisplayName = Resource.Department, Type = FieldType.Dropdown, IsRequired = true, Options = departments }
                };

            case ImportEntity.Appointment:
                return new List<FieldDefinition>
                {
                    new FieldDefinition { Name = nameof(AppointmentDto.DoctorId), DisplayName = Resource.Doctor, Type = FieldType.Dropdown, IsRequired = true, Options = doctors },
                    new FieldDefinition { Name = nameof(AppointmentDto.PatientId), DisplayName = Resource.Patient, Type = FieldType.Dropdown, IsRequired = true, Options = patients },
                    new FieldDefinition { Name = nameof(AppointmentDto.AppointmentDate), DisplayName = Resource.Date, Type = FieldType.Date, IsRequired = true },
                    new FieldDefinition { Name = nameof(AppointmentDto.AppointmentStartTime), DisplayName = Resource.StartTime, Type = FieldType.Time, IsRequired = true },
                    new FieldDefinition { Name = nameof(AppointmentDto.AppointmentEndTime), DisplayName = Resource.EndTime, Type = FieldType.Time, IsRequired = true },
                    new FieldDefinition { Name = nameof(AppointmentDto.Reason), DisplayName = Resource.Reason, Type = FieldType.Dropdown, IsRequired = true, Options = GetReasons() },
                    new FieldDefinition { Name = nameof(AppointmentDto.Notes), DisplayName = Resource.Notes, Type = FieldType.Text, IsRequired = false }
                };

            default:
                return new List<FieldDefinition>();
        }
    }

    public static IEnumerable<SelectItem> GetStatuses()
    {
       var appointmentStatuses = Enum.GetValues(typeof(AppointmentStatus))
        .Cast<AppointmentStatus>()
        .Select(status => new SelectItem
        {
            Value = (int)status,
            Text = status.ToLocalizedString(),
        })
        .ToList();

        return appointmentStatuses;
    }

    public static IEnumerable<SelectItem> GetReasons()
    {
       var appointmentReasons = Enum.GetValues(typeof(AppointmentReason))
        .Cast<AppointmentReason>()
        .Select(reason => new SelectItem
        {
            Value = (int)reason,
            Text = reason.ToLocalizedString(),
        })
        .ToList();

        return appointmentReasons;
    }
}
