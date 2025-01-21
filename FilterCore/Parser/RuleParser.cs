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
                    Errors.Add($"Expect Rule start token at {Peek().Line} : {Peek().Value}");
                    while (true)
                    {
                        if (Peek().type == TokenType.RULE_START || Peek().type == TokenType.CONTINUE|| Peek().type == TokenType.MODIFIER_KEYWORD)
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

        Rule rule = new() { StartToken = start };

        List<TokenType> stopTokens = [TokenType.EOF, TokenType.RULE_START];
        while (!stopTokens.Contains(Peek().type))
        {

            Token current = Peek();
            if (current.type == TokenType.CONTINUE)
            {
                Consume();
                continue;
            }
            if (current.type == TokenType.MODIFIER_KEYWORD)
            {
                RuleNode node = new()
                {
                    Operator = Consume()
                };

                if (Peek().type == TokenType.BOOL_OPERATOR)
                {
                    node.Parameters.Add(Consume());
                }

                while (Peek().type == TokenType.STRING)
                {
                    node.Parameters.Add(Consume());
                }
                rule.Nodes.Add(node);
            }
            else
            {
                Errors.Add($"Expect MODIFIER_KEYWORD but got {current.type} at {current.Line}");
                List<TokenType> set = [TokenType.RULE_START, TokenType.CONTINUE, TokenType.EOF];
                while (set.Contains(Peek().type) == false)
                {
                    Advance();
                }
            }
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
