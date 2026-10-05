namespace BeHealthy.Application.Extensions;

public static class DateTimeExtensions
{
    public static string ToFullDateTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("dd MMM yyyy HH:mm");
    }

    public static string ToShortHumanizedDateString(this DateTime dateTime)
    {
        return dateTime.ToString("dd MMM yyyy");
    }
}
