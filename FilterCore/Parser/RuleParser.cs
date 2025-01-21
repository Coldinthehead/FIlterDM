namespace FilterCore.Parser;

public class ParseException : Exception
{
    public ParseException(string msg) : base(msg)
    {
    }
}

public class RuleParser
{

    private int _currentIndex;

    private List<Token> _tokens;

    public List<string> Errors = [];

    public List<Rule> Parse(List<Token> tokens)
    {
        List<Rule> rules = new();
        _tokens = tokens;
        _currentIndex = 0;

        while (Peek().type != TokenType.EOF)
        {
            switch (Peek().type)
            {
                case TokenType.RULE_START:
                {
                    try
                    {
                        Rule rule = BuildRule();
                        rules.Add(rule);
                    }
                    catch (Exception e)
                    {
                        Errors.Add(e.Message);
                        Advance();
                    }
                }
                break;
                default:
                {
                    Errors.Add($"Xxpect Rule start token at {Peek().Line} : {Peek().Value}");
                    while (true)
                    {
                        if (Peek().type == TokenType.RULE_START)
                        {
                            break;
                        }
                        if (Peek().type == TokenType.EOF)
                        {
                            break;
                        }
                        Advance();
                    }
                    break;
                }
            }
        }

        return rules;
    }

    private Rule BuildRule()
    {
        Token start = Consume();
        Advance();

        Rule rule = new() { StartToken = start };

        List<TokenType> stopTokens = [TokenType.EOF, TokenType.RULE_START, TokenType.CONTINUE];
        while (!stopTokens.Contains(Peek().type))
        {
            Token current = Consume();
            if (current.type == TokenType.MODIFIER_KEYWORD)
            {
                RuleNode node = new()
                {
                    Operator = current
                };
                while (Peek().type != TokenType.NEW_LINE && Peek().type != TokenType.EOF)
                {
                    node.Parameters.Add(Consume());
                }
                rule.Nodes.Add(node);
            }
            else
            {
                Errors.Add($"Expect MODIFIER_KEYWORD but got {current.type} at {current.Line}");
                while (Peek().type != TokenType.NEW_LINE)
                {
                    Advance();
                }
            }
            Advance();
        }
        return rule;
    }

    private void Advance()
    {
        _currentIndex++;
    }

    private Token Consume()
    {
        if (_currentIndex + 1 >= _tokens.Count)
        {
            return _tokens[_tokens.Count - 1];
        }
        return _tokens[_currentIndex++];
    }

    private Token Peek(int count = 0)
    {
        if (_currentIndex + count >= _tokens.Count)
        {
            return _tokens[_tokens.Count - 1];
        }
        return _tokens[_currentIndex + count];
    }
}
