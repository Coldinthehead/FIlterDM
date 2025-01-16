using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace FilterDM.Convertors;

public class SolidHueColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) 
    {
        return new SolidColorBrush(new HsvColor(1, (double)value, 1, 1).ToRgb());
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color.ToHsv().H;
        }
        return null;
    }
}
