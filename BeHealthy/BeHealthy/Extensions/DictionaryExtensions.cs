namespace BeHealthy.Extensions;

public static class DictionaryExtensions
{
    public static T? GetValueOrDefault<T>(this Dictionary<string, object> dict, string key)
    {
        if (dict.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }
}
