using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace FilterDM.Convertors;

public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string enumString && value is Enum enumValue)
        {
            return enumValue.ToString() == enumString;
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true && parameter is string enumString && Enum.IsDefined(targetType, enumString))
        {
            return Enum.Parse(targetType, enumString);
        }
        return null;
    }
}
