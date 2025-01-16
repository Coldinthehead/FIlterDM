using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace FilterDM.Convertors;

public class HsvToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is HsvColor color)
        {
            return new SolidColorBrush(color.ToRgb());
        }
        return null;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush color)
        {
            return color.Color.ToHsv();
        }
        return null;
    }
}
