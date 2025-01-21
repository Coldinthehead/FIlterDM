namespace FilterCore.Parser;

public class Token
{
    public TokenType type;

    public string Value;

    public int Line;

    public override string ToString()
    {
        return $"{Line} : [{type}] {Value}";
    }
}
