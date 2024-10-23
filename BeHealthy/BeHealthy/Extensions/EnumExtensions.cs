using BeHealthy.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BeHealthy.Extensions;

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

    public static string GetBadgeClass(this AppointmentStatus status)
    {
        return status switch
        {
            AppointmentStatus.Scheduled => "bg-info",
            AppointmentStatus.Completed => "bg-success",
            AppointmentStatus.Cancelled => "bg-danger",
            AppointmentStatus.Rescheduled => "bg-warning",
            _ => "bg-secondary"
        };
    }
}
