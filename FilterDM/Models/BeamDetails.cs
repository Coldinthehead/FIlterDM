#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using System;

namespace FilterDM.Models;

public class BeamDetails
{
    public string Color { get; set; }
    public bool IsPermanent { get; set; }

    internal BeamDetails Clone() => new BeamDetails() { Color = Color, IsPermanent= IsPermanent };
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
