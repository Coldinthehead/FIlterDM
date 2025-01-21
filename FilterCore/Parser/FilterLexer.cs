using System.Security.Cryptography;

namespace FilterCore.Parser;

public enum TokenType
{
    EOF,
    STRING,
    RULE_START,
    CONTINUE,
    NEW_LINE,
    MODIFIER_KEYWORD,
    BOOL_OPERATOR,
}

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

public class LexerError : Exception
{
    public LexerError(string message) : base(message)
    {

    }

}

public class FilterLexer
{
    private int _currentIndex = 0;
    private string _input;

    private int _currentLine;

    private Dictionary<string, TokenType> _keywordsMap = new Dictionary<string, TokenType>()
    {
        {"Show", TokenType.RULE_START },
        {"Hide", TokenType.RULE_START },
        {"Continue", TokenType.CONTINUE },
        {"AreaLevel",  TokenType.MODIFIER_KEYWORD},
        {"DropLevel",  TokenType.MODIFIER_KEYWORD},
        {"ItemLevel",  TokenType.MODIFIER_KEYWORD},
        {"Quality",  TokenType.MODIFIER_KEYWORD},
        {"Sockets",  TokenType.MODIFIER_KEYWORD},
        {"StackSize",  TokenType.MODIFIER_KEYWORD},
        {"Height",  TokenType.MODIFIER_KEYWORD},
        {"Width",  TokenType.MODIFIER_KEYWORD},
        {"WaystoneTier",  TokenType.MODIFIER_KEYWORD},
        {"BaseEnergyShield",  TokenType.MODIFIER_KEYWORD},
        {"BaseArmour",  TokenType.MODIFIER_KEYWORD},
        {"BaseEvasion",  TokenType.MODIFIER_KEYWORD },
        {"Rarity", TokenType.MODIFIER_KEYWORD},
        {"Class", TokenType.MODIFIER_KEYWORD },
        {"BaseType", TokenType.MODIFIER_KEYWORD },
        {"Corrupted", TokenType.MODIFIER_KEYWORD},
        {"Mirrored", TokenType.MODIFIER_KEYWORD },
        {"AnyEnchantment", TokenType.MODIFIER_KEYWORD },
        {"DisableDropSound", TokenType.MODIFIER_KEYWORD},
        {"SetFontSize", TokenType.MODIFIER_KEYWORD },
        {"SetBorderColor", TokenType.MODIFIER_KEYWORD },
        {"SetTextColor", TokenType.MODIFIER_KEYWORD },
        {"SetBackgroundColor", TokenType.MODIFIER_KEYWORD },
        {"PlayAlertSound", TokenType.MODIFIER_KEYWORD },
        {"PlayAlertSoundPositional", TokenType.MODIFIER_KEYWORD },
        {"MinimapIcon", TokenType.MODIFIER_KEYWORD },
        {"PlayEffect", TokenType.MODIFIER_KEYWORD },
        {"HasExplicitMod", TokenType.MODIFIER_KEYWORD },
    };

    public List<Token> BuildTokens(string input)
    {
        _input = input;
        _currentIndex = 0;
        return Parse();
    }

    private List<Token> Parse()
    {
        List<Token> result = [];
        _currentLine = 1;

        while (Peek() != '\0' && _currentIndex < _input.Length)
        {
            switch (Peek())
            {
                case '#':
                {
                    while (Peek() != '\0' && Peek() != '\n')
                    {
                        Advance();
                    }
                }
                break;
                case '\n':
                {
                    result.Add(new Token()
                    {
                        type = TokenType.NEW_LINE,
                    });
                }
                break;
                case '"':
                {
                    Advance();
                    List<char> chars = [];
                    int line = _currentLine;
                    while (Peek() != '"')
                    {
                        if (Peek() == '\n')
                        {
                            throw new LexerError($"Multipline string at {line}!");
                        }
                        if (Peek() == '\0')
                        {
                            throw new LexerError($"Unterminated string at {line}");
                        }

                        chars.Add(Peek());
                        Advance();
                    }
                    string word = string.Join("", chars);
                    result.Add(new Token()
                    {
                        type = TokenType.STRING,
                        Line = _currentLine,
                        Value = word
                    });
                }
                break;
                case '=':
                {
                    if (Peek(1) == '=')
                    {
                        Advance();
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Value = "==",
                            Line = _currentLine,
                        });
                    }
                    else
                    {
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Value = "=",
                            Line = _currentLine,
                        });
                    }
                }
                break;
                case '>':
                {
                    if (Peek(1) == '=')
                    {
                        Advance();
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Value = ">=",
                            Line = _currentLine,
                        });
                    }
                    else
                    {
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Value = ">",
                            Line = _currentLine,
                        });
                    }
                }
                break;
                case '<':
                {
                    if (Peek(1) == '=')
                    {
                        Advance();
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Value = "<=",
                            Line = _currentLine,
                        });
                    }
                    else
                    {
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Value = "<",
                            Line = _currentLine,
                        });
                    }
                }
                break;
                default:
                {
                    List<char> chars = new List<char>();
                    if (char.IsDigit(Peek()))
                    {
                        while (char.IsDigit(Peek()))
                        {
                            chars.Add(Peek());
                            Advance();
                        }
                    }
                    else
                    {
                        while (IsStringCharacter(Peek()))
                        {
                            chars.Add(Peek());
                            Advance();
                        }
                    }
                    string word = string.Join("", chars);
                    if (_keywordsMap.ContainsKey(word))
                    {
                        result.Add(new Token()
                        {
                            type = _keywordsMap[word],
                            Line = _currentLine,
                            Value = word,
                        });
                    }
                    else
                    {
                        result.Add(new Token()
                        {
                            type = TokenType.STRING,
                            Line = _currentLine,
                            Value = word,
                        });
                    }
                }
                break;
            }
            Advance();
        }


        result.Add(new Token() { type = TokenType.EOF });
        return result;
    }

    private bool IsStringCharacter(char currentCharacter)
    {
        return char.IsLetter(currentCharacter);
    }

    private void Advance()
    {
        if (Peek() == '\n')
        {
            _currentLine++;
        }
        if (_currentIndex + 1 <= _input.Length)
        {
            _currentIndex++;
        }
    }

    private char Peek(int amount = 0)
    {
        if (_currentIndex + amount >= _input.Length)
        {
            return '\0';
        }
        return _input[_currentIndex + amount];
    }
}
public class Rule
{
    public Token StartToken;
    public List<RuleNode> Nodes = [];
}

public class RuleNode
{
    public Token @Operator;

    public List<Token> Parameters = [];
}
