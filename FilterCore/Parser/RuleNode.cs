namespace FilterCore.Parser;

public class RuleNode
{
    public Token @Operator;

    public ModifierType ModType; 

    public List<Token> Parameters = [];

    public List<ParameterType> ParameterTypes = [];

    public Dictionary<string, object> TypedParameters { get; internal set; }

    public ParameterType GetParameterMeta(int index)
    {
        return ParameterTypes[index];
    }
    public ModifierType GetOperatorMetaType()
    {
        return ModType;
    }
}
