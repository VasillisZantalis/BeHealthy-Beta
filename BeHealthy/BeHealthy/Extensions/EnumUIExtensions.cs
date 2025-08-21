using BeHealthy.Models;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Extensions;

public static class EnumUIExtensions
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

    public static List<SelectItem> GetEnumAsSelect<T>(bool? addPleaseSelect = false) where T : Enum
    {
        var selectItems = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(value => new SelectItem
            {
                Value = Convert.ToInt32(value),
                Text = value.ToLocalizedString()
            })
            .ToList();

        if (addPleaseSelect == true)
            selectItems.Insert(0, new SelectItem { Text = Resource.PleaseSelect, Value = 0 });

        return selectItems;
    }
}
