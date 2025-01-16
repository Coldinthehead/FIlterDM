using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace FilterDM.Convertors;


public class HsvToHexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is HsvColor color)
        {
            return $"#{color.ToRgb().ToUInt32():X}";
        }
        return "Invalid Color";
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string hex)
        {
            if (Color.TryParse(hex, out Color c))
            {
                return c.ToHsv();
            }
        }
        return Colors.Magenta.ToHsv();
    }
}
