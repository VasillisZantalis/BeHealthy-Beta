using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Helpers;

public static class AppSettingsConverterHelper
{
    public static bool GetBooleanValue(this AppSettingDto setting)
    {
        bool.TryParse(setting.Value, out var res);
        return res;
    }

    public static DateTime GetValueToDateTime(this AppSettingDto setting)
    {
        DateTime.TryParse(setting.Value, out var res);
        return res;
    }

    public static string GetStringValue(this AppSettingDto setting)
    {
        return setting.Value;
    }

    public static int GetIntValue(this AppSettingDto setting)
    {
        int.TryParse(setting.Value, out var res);
        return res;
    }
}
