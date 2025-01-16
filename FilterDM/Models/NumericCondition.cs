#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;

public class NumericCondition : ConditionModel
{
    public string ValueName { get; set; } = string.Empty;
    public NumericConditionSign UseEquals { get; set; }
    public int Number { get; set; }

    public NumericCondition Clone()
    {
        NumericCondition clone = new();
        clone.ValueName = ValueName;
        clone.UseEquals = UseEquals;
        clone.Number = Number;
        return clone;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
