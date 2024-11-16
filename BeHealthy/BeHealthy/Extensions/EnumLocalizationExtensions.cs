using BeHealthy.Locales;
using Microsoft.Extensions.Localization;

namespace BeHealthy.Extensions;

public static class EnumLocalizationExtensions
{
    public static string ToLocalizedString<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        var enumName = enumValue.ToString();

        var resourceKey = $"{enumType.Name}_{enumName}";

        var resourceManager = Resource.ResourceManager;
        var localizedString = resourceManager.GetString(resourceKey);

        return string.IsNullOrEmpty(localizedString) ? enumName : localizedString;
    }
}
