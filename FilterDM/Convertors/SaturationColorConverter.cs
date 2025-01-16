using Avalonia.Controls;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using Avalonia.Controls.Shapes;

namespace FilterDM.Convertors;

public class SaturationColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return CreateHueGradient((double)value);
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }


    private static VisualBrush CreateHueGradient(double hue)
    {
        var color = new HsvColor(1, hue, 1, 1);
        // Canvas для візуалізації
        var visualCanvas = new Canvas
        {
            Width = 1,
            Height = 1,
            Background = Brushes.Black,
            Children =
        {
            // Прямокутник із градієнтним заповненням
            new Rectangle
            {
                Width = 1,
                Height = 1,
                Fill = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Colors.White, 0),
                        new GradientStop(color.ToRgb(), 1)
                    }
                },
                OpacityMask = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Colors.White, 0),
                        new GradientStop(Colors.Transparent, 1)
                    }
                }
            }
        }
        };

        // Створюємо VisualBrush
        return new VisualBrush
        {
            Visual = visualCanvas,
            TileMode = TileMode.None
        };
    }
}
