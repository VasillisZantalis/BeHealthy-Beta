namespace BeHealthy.Client.Extensions;

public static class ValueExtensions
{
    public static int? ToNullable(this string value)
    {
        return int.TryParse(value, out var result) ? result : null;
    }
}
