#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using System;

namespace FilterDM.Models;

public class MinimapIconDetails
{
    public string Color { get; set; }
    public string Shape { get; set; }
    public string Size { get; set; }

    internal MinimapIconDetails? Clone() => new() { Color = Color, Shape = Shape, Size = Size };
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
