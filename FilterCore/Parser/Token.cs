namespace FilterCore.Parser;

public class Token
{
    public TokenType type;

    public readonly string Value;

    public int Line;

    public Token(string value)
    {
        Value = value;
    }
    public override string ToString()
    {
        return $"{Line} : [{type}] {Value}";
    }
}
