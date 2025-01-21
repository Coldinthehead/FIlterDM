
namespace FilterCore.Parser;

public class ModifierResolver
{
    public bool Resolve(RuleNode node)
    {
        if (Enum.TryParse(typeof(ModifierType), node.Operator.Value, out var result))
        {
            node.ModType = (ModifierType)result;
            return true;
        }
        return false;
    }
}

public class TypeResolver
{
    private readonly Dictionary<string, ParameterType> _defaults = new Dictionary<string, ParameterType>()
    {
        {"True", ParameterType.Bool_literal},
        {"False", ParameterType.Bool_literal},
        {"Temp", ParameterType.Temp},
        {"Red", ParameterType.Color},
        {"Green", ParameterType.Color},
        {"Blue", ParameterType.Color},
        {"Brown", ParameterType.Color},
        {"White", ParameterType.Color},
        {"Yellow", ParameterType.Color},
        {"Cyan", ParameterType.Color},
        {"Grey", ParameterType.Color},
        {"Orange", ParameterType.Color},
        {"Pink", ParameterType.Color},
        {"Purple", ParameterType.Color},
        {"Circle", ParameterType.Shape},
        {"Diamond", ParameterType.Shape},
        {"Hexagon", ParameterType.Shape},
        {"Square", ParameterType.Shape},
        {"Start", ParameterType.Shape},
        {"Triangle", ParameterType.Shape},
        {"Cross", ParameterType.Shape},
        {"Moon", ParameterType.Shape},
        {"Raindrop", ParameterType.Shape},
        {"Kite", ParameterType.Shape},
        {"Pentagon", ParameterType.Shape},
        {"UpsideDownHouse", ParameterType.Shape},
        {"None", ParameterType.None},
        {">", ParameterType.Bool_operator},
        {"<", ParameterType.Bool_operator},
        {"<=", ParameterType.Bool_operator},
        {">=", ParameterType.Bool_operator},
        {"=", ParameterType.Bool_operator},
        {"==", ParameterType.Bool_operator},




    };
    public bool Resolve(RuleNode node)
    {
        foreach (var parameter in node.Parameters)
        {
            node.ParameterTypes.Add(ResolveSingle(parameter));
        }

        return CanHaveParameters(node);
    }

    public bool CanHaveParameters(RuleNode node)
    {
        return true;
    }

    private ParameterType ResolveSingle(Token parameter)
    {
        string value = parameter.Value;
        if (_defaults.ContainsKey(value))
        {
            return _defaults[value];
        }
        if (int.TryParse(value, out  _))
        {
            return ParameterType.Number;
        }
        return ParameterType.Name;
    }
}
