using System;

namespace FilterDM.ViewModels.EditPage;

public class NumericFilterHelper
{
    private readonly Action<bool> _useAction;

    public string Name { get; private set; }
    public string ShortName { get; private set; }

    public int MaxValue { get; private set; }

    public NumericFilterType Type { get; set; }

    public NumericFilterHelper(NumericFilterType type, string longName, string shortName, int maxValue, Action<bool> useAction)
    {
        Type = type;
        Name = longName;
        ShortName = shortName;
        _useAction = useAction;
        MaxValue = maxValue;
    }

    public void Add() => _useAction.Invoke(true);
    public void Remove() => _useAction.Invoke(false);

}
