namespace FilterCore.Conditions.Impl;

public class StackSizeCondition : FilteringCondition
{
    public readonly int Count;
    public StackSizeCondition(int count) : base("StackSize >=")
    {
        Count = count;
    }

    public override string DumpSting()
    {
        return $"{Helper.Tab} StackSize >= {Count}";
    }
}
