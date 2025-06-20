using BeHealthy.Domain;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BeHealthy.Application.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var fieldInfo = enumValue?.GetType().GetField(enumValue.ToString());

        if (fieldInfo != null)
        {
            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue?.ToString() ?? string.Empty;
        }

        return enumValue?.ToString() ?? string.Empty;
    }

    public static string GetBadgeClass(this AppointmentStatus status) => status switch
    {
        AppointmentStatus.Scheduled => "bg-info",
        AppointmentStatus.Completed => "bg-success",
        AppointmentStatus.Cancelled => "bg-danger",
        AppointmentStatus.Rescheduled => "bg-warning",
        _ => "bg-secondary"
    };

    public static string GetStatusColor(this AppointmentStatus status) => status switch
    {
        AppointmentStatus.Scheduled => "#4094f5",
        AppointmentStatus.Completed => "#1b942f",
        AppointmentStatus.Cancelled => "#e82113",
        AppointmentStatus.Rescheduled => "#e8b613",
        _ => "#4094f5"
    };
}
