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
                    Errors.Add($"expect Rule start token at {Peek().Line} : {Peek().Value}");
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

        Rule rule = new() { StartToken = start };

        while (true)
        {
            var token = Peek();

            if (token.type == TokenType.EOF || token.type == TokenType.RULE_START)
            {
                break;
            }
            if (token.type == TokenType.CONTINUE)
            {
                Consume();
                break;
            }


            switch (token.type)
            {
                case TokenType.COLOR_DECORATOR:
                {
                    RuleNode node = ParseColor();
                    rule.Nodes.Add(node);
                }
                break;
                case TokenType.TEXT_SIZE_DECORATOR:
                {
                    // parse font size
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    node.Parameters.Add(Consume());
                    rule.Nodes.Add(node);
                }
                break;

                case TokenType.BEAM_DECORATOR:
                {
                    //parse beam
                    RuleNode node = new RuleNode()
                    {
                        Operator = Consume()
                    };
                    node.Parameters.Add(Consume());
                    if (Peek().type == TokenType.TEMP)
                    {
                        node.Parameters.Add(Consume());
                    }
                    rule.Nodes.Add(node);
                }
                break;
                case TokenType.MINIMAP_DECORATOR:
                {
                    //minimap 
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    node.Parameters.Add(Consume());
                    node.Parameters.Add(Consume());
                    node.Parameters.Add(Consume());
                    rule.Nodes.Add(node);
                }
                break;

                case TokenType.SOUND_DECORATOR:
                {
                    //sound
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    node.Parameters.Add(Consume());
                    if (Peek().type == TokenType.NUMBER)
                    {
                        node.Parameters.Add(Consume());
                    }
                    rule.Nodes.Add(node);
                }
                break;

                case TokenType.NUMERIC_DECORATOR:
                {
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    if (Peek().type == TokenType.BOOL_OPERATOR)
                    {
                        node.Parameters.Add(Consume());
                    }
                    node.Parameters.Add(Consume());
                    rule.Nodes.Add(node);
                }
                break;

                case TokenType.CLASS_DECORATOR:
                {
                    //class 
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
                break;
                case TokenType.TYPE_DECORATOR:
                {
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };


                    if (Peek().type == TokenType.BOOL_OPERATOR)
                    {
                        node.Parameters.Add(Consume());
                    }
                    while (Peek().type == TokenType.STRING || Peek().type == TokenType.SHAPE )
                    {
                        node.Parameters.Add(Consume());
                    }
                    rule.Nodes.Add(node);
                }
                break;

                case TokenType.SINGLE_DECORATOR:
                {
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    rule.Nodes.Add(node);
                }
                break;

                case TokenType.RARITY_DECORATOR:
                {
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    if (Peek().type == TokenType.BOOL_OPERATOR)
                    {
                        node.Parameters.Add(Consume());
                    }
                    
                    while (Peek().type == TokenType.RARITY_TYPE)
                    {
                        node.Parameters.Add(Consume());
                    }
                   
                    rule.Nodes.Add(node);
                }
                break;
                case TokenType.BOOL_DECORATOR:
                {
                   RuleNode node = new()
                   {
                        Operator = Consume() 
                   };
                    node.Parameters.Add(Consume());
                    rule.Nodes.Add(node);
                }
                break;
                case TokenType.EXLICIT_MOD_DECORATOR:
                {
                    RuleNode node = new()
                    {
                        Operator = Consume()
                    };
                    if (Peek().type == TokenType.BOOL_OPERATOR)
                    {
                        node.Parameters.Add(Consume());
                        node.Parameters.Add(Consume());
                    }
                    while (Peek().type == TokenType.STRING)
                    {
                        node.Parameters.Add(Consume());
                    }
                    rule.Nodes.Add(node); 
                }
                break;
                default:
                throw new ParseException($"unexpected token : {token.type} at {token.Line}");
                break;
            }

        }

        return rule;
    }

    private RuleNode ParseColor()
    {
        RuleNode node = new RuleNode()
        {
            Operator = Consume()
        };
        node.Parameters.Add(Consume());
        node.Parameters.Add(Consume());
        node.Parameters.Add(Consume());
        if (Peek().type == TokenType.NUMBER)
        {
            node.Parameters.Add(Consume());
        }
        return node;
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
