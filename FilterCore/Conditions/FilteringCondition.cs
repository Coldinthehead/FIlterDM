namespace FilterCore.Conditions;

public abstract class FilteringCondition
{
    public readonly string Name;

    public string ParentName
    {
        get => _parentName + "." + Name;
        set => _parentName = value;
    }
    private string _parentName;

    public FilteringCondition(string name)
    {
        Name = name;
        _parentName = string.Empty;
    }
    public abstract string DumpSting();
}
