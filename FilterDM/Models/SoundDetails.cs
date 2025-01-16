#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using System;

namespace FilterDM.Models;

public class SoundDetails
{
    public int Sample { get; set; }

    public int Volume { get; set; }

    internal SoundDetails? Clone() => new() {Sample = Sample, Volume = Volume };
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
