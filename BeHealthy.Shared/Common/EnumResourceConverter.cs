using BeHealthy.Shared.Locales;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace BeHealthy.Shared.Common;

public class EnumResourceConverter : EnumConverter
{
    public EnumResourceConverter(Type type) : base(type) { }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is Enum enumValue)
        {
            return ConvertToDisplayString(enumValue);
        }

        if (value == null || destinationType == null)
        {
            return base.ConvertTo(context, culture, value, destinationType!);
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    public static string ConvertToDisplayString(Enum enumValue)
    {
        var enumTypeName = enumValue.GetType().Name;
        var enumName = enumValue.ToString();
        var resourceKey = $"{enumTypeName}_{enumName}";

        var field = typeof(Resource).GetField(resourceKey, BindingFlags.Public | BindingFlags.Static);
        var displayString = field?.GetValue(null) as string;

        return string.IsNullOrEmpty(displayString) ? enumName : displayString;
    }
}
