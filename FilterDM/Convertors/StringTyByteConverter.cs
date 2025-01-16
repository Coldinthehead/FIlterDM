using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace FilterDM.Convertors;

public class StringTyByteConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int result)
        {
            return result.ToString();
        }
        return "NaN";
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string strItem && int.TryParse(strItem, out int result))
        {
            return result;
        }
        return 0;
    }
}
