using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Helpers;

public static class AppSettingsConverterHelper
{
    public static bool GetBooleanValue(AppSetting setting)
    {
        bool.TryParse(setting.Value, out var res);
        return res;
    }

    public static DateTime GetValueToDateTime(AppSetting setting)
    {
        DateTime.TryParse(setting.Value, out var res);
        return res;
    }

    public static string GetStringValue(AppSetting setting)
    {
        return setting.Value;
    }

    public static int GetIntValue(AppSetting setting)
    {
        int.TryParse(setting.Value, out var res);
        return res;
    }
}
