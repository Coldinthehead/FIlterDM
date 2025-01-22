using Avalonia.Media;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;

public class SerializableColor
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Alpha { get; set; }

    public static SerializableColor FromAvalonia(Color color)
    {
        return new SerializableColor()
        {
            Red = color.R,
            Green = color.G,
            Blue = color.B,
            Alpha = color.A,
        };
    }


    internal Color ToAvalonia() => Color.FromArgb((byte)Alpha, (byte)Red, (byte)Green, (byte)Blue);
    internal SerializableColor Clone()
    {
        return new SerializableColor()
        {
            Red = Red,
            Green = Green,
            Blue = Blue,
            Alpha = Alpha,
        };
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
