using BeHealthy.Shared.Dtos.Common;

namespace BeHealthy.Frontend.Helpers;

public static class AppSettingsConverterHelper
{
    public static bool GetBooleanValue(this AppSettingResponse setting)
    {
        bool.TryParse(setting.Value, out var res);
        return res;
    }

    public static DateTime GetValueToDateTime(this AppSettingResponse setting)
    {
        DateTime.TryParse(setting.Value, out var res);
        return res;
    }

    public static string GetStringValue(this AppSettingResponse setting)
    {
        return setting.Value;
    }

    public static int GetIntValue(this AppSettingResponse setting)
    {
        int.TryParse(setting.Value, out var res);
        return res;
    }
}
