using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Helpers;

public static class AppSettingsConverterHelper
{
    public static bool GetBooleanValue(this AppSetting setting)
    {
        bool.TryParse(setting.Value, out var res);
        return res;
    }

    public static DateTime GetValueToDateTime(this AppSetting setting)
    {
        DateTime.TryParse(setting.Value, out var res);
        return res;
    }

    public static string GetStringValue(this AppSetting setting)
    {
        return setting.Value;
    }

    public static int GetIntValue(this AppSetting setting)
    {
        int.TryParse(setting.Value, out var res);
        return res;
    }
}
