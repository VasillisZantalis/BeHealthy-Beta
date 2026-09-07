using BeHealthy.Frontend.Models;
using BeHealthy.Shared.Common;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Frontend.Extensions;

public static class EnumUIExtensions
{
    public static string ToDisplayString<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        return EnumResourceConverter.ConvertToDisplayString(enumValue);
    }

    public static List<SelectItem> GetEnumAsSelect<T>(bool? addPleaseSelect = false) where T : Enum
    {
        var selectItems = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(value => new SelectItem
            {
                Value = Convert.ToInt32(value),
                Text = value.ToDisplayString()
            })
            .ToList();

        if (addPleaseSelect == true)
            selectItems.Insert(0, new SelectItem { Text = Resource.PleaseSelect, Value = 0 });

        return selectItems;
    }
}
