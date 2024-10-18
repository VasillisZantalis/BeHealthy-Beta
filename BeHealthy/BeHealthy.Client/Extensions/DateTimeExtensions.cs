namespace BeHealthy.Client.Extensions;

public static class DateTimeExtensions
{
    public static string ToFullDateTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("dd MMM yyyy HH:mm");
    }

    public static string ToShortDateString(this DateTime dateTime)
    {
        return dateTime.ToString("dd MMM yyyy");
    }
}
