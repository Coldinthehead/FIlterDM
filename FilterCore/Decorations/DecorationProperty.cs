using FilterCore.Enums;
using System.Drawing;

namespace FilterCore.Decorations;

public readonly struct DecorationProperty
{
    public readonly string Content;
    public readonly DecorationType @Type;

    private DecorationProperty(string content, DecorationType type)
    {
        Content = content;
        Type = type;
    }

    public static DecorationProperty FontSize(int size)
    {
        return new DecorationProperty($"SetFontSize {size}", DecorationType.FontSize);
    }

    public static DecorationProperty BorderColor(Color color)
    {
        return new DecorationProperty($"SetBorderColor {color.R} {color.G} {color.B} {color.A}"
            , DecorationType.BorderColor);
    }

    public static DecorationProperty BackgroundColor(Color color)
    {
        return new DecorationProperty($"SetBackgroundColor {color.R} {color.G} {color.B} {color.A}"
            , DecorationType.BackgroundColor);
    }

    public static DecorationProperty TextColor(Color color)
    {
        return new DecorationProperty($"SetTextColor {color.R} {color.G} {color.B} {color.A}"
            , DecorationType.TextColor);
    }

    public static DecorationProperty MinimapIcon(MinimapIconSize size
        , StaticGameColor color
        , MinimapIconShape shape)
    {
        return new DecorationProperty($"MinimapIcon {(int)size} {color.ToString()} {shape.ToString()}"
            , DecorationType.MinimapIcon);
    }

    public static DecorationProperty PermanentBeam(StaticGameColor color)
    {
        return new DecorationProperty($"PlayEffect {color.ToString()}", DecorationType.Beam);
    }

    public static DecorationProperty TempBeam(StaticGameColor color)
    {
        return new DecorationProperty($"PlayEffect {color.ToString()} Temp", DecorationType.Beam);
    }

    public static DecorationProperty AlertSound(Sound sound, int volume)
    {

        return new DecorationProperty($"PlayAlertSound {(int)sound} {volume}", DecorationType.AlertSound);
    }

    public static DecorationProperty PositionalAllertSound(int soundId, int volume)
    {
        if (soundId < 1 || soundId > 16)
        {
            throw new ArgumentException($"Sound id out of range 1-16 : {soundId}");
        }

        return new DecorationProperty($"PlayAlertSoundPositional {soundId} {volume}", DecorationType.AlertSound);
    }
}
