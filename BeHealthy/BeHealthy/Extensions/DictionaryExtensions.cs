namespace BeHealthy.Extensions;

public static class DictionaryExtensions
{
    public static T? GetValueOrDefault<T>(this Dictionary<string, object> dict, string key)
    {
        if (!dict.TryGetValue(key, out var value) || value == null)
            return default;

        // If it's already the right type
        if (value is T typedValue)
            return typedValue;

        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        try
        {
            if (value is DateTime dt && targetType == typeof(DateOnly))
                return (T)(object)DateOnly.FromDateTime(dt);

            if (value is TimeSpan ts && targetType == typeof(TimeOnly))
                return (T)(object)TimeOnly.FromTimeSpan(ts);

            if (value is string str)
            {
                if (targetType.IsEnum)
                    return (T)Enum.Parse(targetType, str);

                return (T)Convert.ChangeType(str, targetType);
            }

            // Fallback: try normal conversion
            return (T)Convert.ChangeType(value, targetType);
        }
        catch
        {
            return default;
        }
    }
}
