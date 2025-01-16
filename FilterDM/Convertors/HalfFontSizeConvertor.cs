using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace FilterDM.Convertors;

public class HalfFontSizeConvertor : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var type = value.GetType();
        if (value is System.Single d)
        {
            return d / 2;
        }
        return 12;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
