using BeHealthy.Shared.Locales;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace BeHealthy.Shared.Common;

public class EnumResourceConverter : EnumConverter
{
    private static readonly ResourceManager _resourceManager = Resource.ResourceManager;

    public EnumResourceConverter(Type type) : base(type) { }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is Enum enumValue)
        {
            return ConvertToLocalizedString(enumValue, culture);
        }

        if (value == null || destinationType == null)
        {
            return base.ConvertTo(context, culture, value, destinationType!);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }


    public static string ConvertToLocalizedString(Enum enumValue, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        var enumTypeName = enumValue.GetType().Name;
        var enumName = enumValue.ToString();
        var resourceKey = $"{enumTypeName}_{enumName}";

        var localizedString = _resourceManager.GetString(resourceKey, culture);

        return string.IsNullOrEmpty(localizedString) ? enumName : localizedString;
    }
}
