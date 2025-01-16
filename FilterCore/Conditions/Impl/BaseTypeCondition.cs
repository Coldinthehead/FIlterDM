using System.Text;

namespace FilterCore.Conditions.Impl;

public class BaseTypeCondition : FilteringCondition
{
    public readonly List<string> BaseTypes = [];

    public BaseTypeCondition(IEnumerable<string> types) : base("BaseTypeCondition")
    {
        BaseTypes.AddRange(types);
    }

    public BaseTypeCondition AddType(string type)
    {
        if (BaseTypes.Contains(type))
        {
            throw new ArgumentException($"BaseType with name {type} already contains in block {Name}");
        }
        BaseTypes.Add(type);
        return this;
    }

    public override string DumpSting()
    {
        StringBuilder sb = new();
        sb.Append($"{Helper.Tab}BaseType");
        foreach (string type in BaseTypes)
        {
            sb.Append($" \"{type}\"");
        }
        return sb.ToString();
    }
}
