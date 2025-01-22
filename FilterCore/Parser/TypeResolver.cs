
namespace FilterCore.Parser;

public class TypeResolveException : Exception
{
    public TypeResolveException(string message) : base(message)
    {
        
    }
}

public class ModifierResolver
{
    public bool Resolve(RuleNode node)
    {
        if (Enum.TryParse(typeof(ModifierType), node.Operator.Value, out var result))
        {
            node.ModType = (ModifierType)result;

            if (node.ModType == ModifierType.None)
            {
                throw new Exception($"Unknows Modifier Type at {node.Operator} at {node.Operator.Line}");
            }
            return true;
        }
        return false;
    }

    public Dictionary<string,object> ResolveAgrTypes(List<Token> parameters , ModifierType type)
    {
        Dictionary<string, object> res = [];
        switch (type)
        {
            case ModifierType.None:
            break;
            case ModifierType.AreaLevel:
            case ModifierType.ItemLevel:
            case ModifierType.DropLevel:
            case ModifierType.Quality:
            case ModifierType.Height:
            case ModifierType.Width:
            case ModifierType.StackSize:
            case ModifierType.BaseAmrmour:
            case ModifierType.BaseEvasion:
            case ModifierType.BaseEnergyShield:
            {
                if (parameters.Count == 1)
                {
                    res["op"] = "=";
                    res["value"] = int.Parse(parameters[1].Value);
                }
                else
                {
                    res["op"] = parameters[0].Value;
                    res["value"] = int.Parse(parameters[1].Value);
                }
            }
            break;
            case ModifierType.Rarity:
            break;
            case ModifierType.Class:
            break;
            case ModifierType.BaseType:
            break;
            case ModifierType.Sockets:
            case ModifierType.WaystoneTier:
            break;
            break;
            break;
            case ModifierType.AnyEnchantment:
            break;
            case ModifierType.HasEnchantment:
            break;
            break;
            case ModifierType.Corrupted:
            break;
            case ModifierType.Mirrored:
            break;
            case ModifierType.SetBorderColor:
            break;
            case ModifierType.SetTextColor:
            break;
            case ModifierType.SetBackgroundColor:
            break;
            case ModifierType.SetFontSize:
            break;
            case ModifierType.PlayAlertSound:
            break;
            case ModifierType.PlayAlertSoundPositional:
            break;
            case ModifierType.DisableDropSound:
            break;
            case ModifierType.EnableDropSound:
            break;
            case ModifierType.CustomAlertSound:
            break;
            case ModifierType.MinimapIcon:
            break;
            case ModifierType.PlayEffect:
            break;
        }

        return res;
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
