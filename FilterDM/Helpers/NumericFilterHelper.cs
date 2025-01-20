using FilterDM.Enums;
using System;

namespace FilterDM.Helpers;

public class NumericFilterHelper
{
    private readonly Action<bool> _useAction;

    public string Name { get; private set; }
    public string ShortName { get; private set; }

    public int MaxValue { get; private set; }

    public NumericFilterType Type { get; set; }

    public NumericFilterHelper(NumericFilterType type, string longName, string shortName, int maxValue)
    {
        Type = type;
        Name = longName;
        ShortName = shortName;
        MaxValue = maxValue;
    }
}
