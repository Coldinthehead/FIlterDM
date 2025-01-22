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

        Errors.Clear();
        int start = 0;
        if (tokens.Count > 0 && tokens.First().type != TokenType.RULE_START)
        {
             Errors.Add($"Expect Show/Hide but got {tokens.First()}");
             while (tokens[start].type != TokenType.RULE_START)
            {
                start++;
                if (start >= tokens.Count)
                {
                    break;
                }
            }
        }

        for (int i = start; i < tokens.Count;i++)
        {
            Token token = tokens[i];
            switch (token.type)
            {
                case TokenType.RULE_START:
                {
                    try
                    {
                        Rule r = BuildRule(tokens, i);
                        rules.Add(r);
                    }
                    catch (ParseException ex)
                    {
                        Errors.Add(ex.Message);
                    }
                }
                break;
                default:
                continue;
            }
        }   

        return rules;
    }

    private Rule BuildRule(List<Token> stream, int index)
    {
        Rule result = new()
        {
            StartToken = stream[index]
        };
        index++;
        bool finish = false;
        while (index < stream.Count )
        {
            Token current = stream[index];
            switch (current.type)
            {
                case TokenType.BOOL_OPERATOR:
                case TokenType.STRING:
                {
                    Errors.Add($"Expect RuleModifier but got {current} at {current.Line}");
                    index++;
                    continue;
                }
                case TokenType.EOF:
                case TokenType.RULE_START:
                case TokenType.CONTINUE:
                {
                    finish = true;
                }
                break;
                case TokenType.MODIFIER_KEYWORD:
                {
                    RuleNode node = new RuleNode()
                    {
                        Operator = stream[index++]
                    };
                    if (index < stream.Count && stream[index].type == TokenType.BOOL_OPERATOR)
                    {
                        node.Parameters.Add(stream[index]);
                        index++;
                    }
                    while (index < stream.Count && stream[index].type == TokenType.STRING)
                    {
                        node.Parameters.Add(stream[index]);
                        index++;
                    }
                    result.Nodes.Add(node);
                }
                break;
 
            }
            if (finish)
            {
                break;
            }
        }
        return result;
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
