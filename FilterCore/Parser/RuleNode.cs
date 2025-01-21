namespace FilterCore.Parser;

public class RuleNode
{
    public Token @Operator;

    public ModifierType ModType; 

    public List<Token> Parameters = [];

    public List<ParameterType> ParameterTypes = [];

    public ParameterType GetParameterMeta(int index)
    {
        return ParameterTypes[index];
    }
    public ModifierType GetOperatorMetaType()
    {
        return ModType;
    }
}
