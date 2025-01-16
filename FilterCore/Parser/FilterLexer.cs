using System.Security.Cryptography;

namespace FilterCore.Parser;

public enum TokenType
{
    EOF,
    NUMBER,
    BOOL,
    STRING,
    BOOL_OPERATOR,
    COLOR_DECORATOR,
    COLOR,
    SHAPE,
    RARITY_DECORATOR,
    BOOL_DECORATOR,
    RARITY_TYPE,
    RULE_START,
    NUMERIC_DECORATOR,
    CLASS_DECORATOR,
    TYPE_DECORATOR,
    SOUND_DECORATOR,
    SINGLE_DECORATOR,
    MINIMAP_DECORATOR,
    TEXT_SIZE_DECORATOR,
    BEAM_DECORATOR,
    TEMP,
    CONTINUE,

    EXLICIT_MOD_DECORATOR,
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
    private char _currentCharacter;

    private int _currentLine;

    private Dictionary<string, TokenType> _keywordsMap = new Dictionary<string, TokenType>()
    {
        {"Show", TokenType.RULE_START },
        {"Hide", TokenType.RULE_START },
        {"Continue", TokenType.CONTINUE },

        {"AreaLevel",  TokenType.NUMERIC_DECORATOR},
        {"DropLevel",  TokenType.NUMERIC_DECORATOR},
        {"ItemLevel",  TokenType.NUMERIC_DECORATOR},
        {"Quality",  TokenType.NUMERIC_DECORATOR},
        {"Sockets",  TokenType.NUMERIC_DECORATOR},
        {"StackSize",  TokenType.NUMERIC_DECORATOR},
        {"Height",  TokenType.NUMERIC_DECORATOR},
        {"Width",  TokenType.NUMERIC_DECORATOR},
        {"WaystoneTier",  TokenType.NUMERIC_DECORATOR},
        {"BaseEnergyShield",  TokenType.NUMERIC_DECORATOR},
        {"BaseArmour",  TokenType.NUMERIC_DECORATOR},
        {"BaseEvasion",  TokenType.NUMERIC_DECORATOR},

        {"Rarity",  TokenType.RARITY_DECORATOR},
        {"Normal",  TokenType.RARITY_TYPE},
        {"Magic",  TokenType.RARITY_TYPE},
        {"Rare",  TokenType.RARITY_TYPE},
        {"Unique",  TokenType.RARITY_TYPE},

        {"Class", TokenType.CLASS_DECORATOR },

        {"BaseType", TokenType.TYPE_DECORATOR },

        {"Corrupted", TokenType.BOOL_DECORATOR },
        {"Mirrored", TokenType.BOOL_DECORATOR },
        {"AnyEnchantment", TokenType.BOOL_DECORATOR },
        {"DisableDropSound", TokenType.BOOL_DECORATOR },


        {"SetFontSize", TokenType.TEXT_SIZE_DECORATOR },

        {"SetBorderColor", TokenType.COLOR_DECORATOR },
        {"SetTextColor", TokenType.COLOR_DECORATOR },
        {"SetBackgroundColor", TokenType.COLOR_DECORATOR },

        {"PlayAlertSound", TokenType.SOUND_DECORATOR },
        {"PlayAlertSoundPositional", TokenType.SOUND_DECORATOR },


        {"MinimapIcon", TokenType.MINIMAP_DECORATOR },

        {"PlayEffect", TokenType.BEAM_DECORATOR },



        {"Red", TokenType.COLOR },
        {"Green", TokenType.COLOR },
        {"Blue", TokenType.COLOR },
        {"Brown", TokenType.COLOR },
        {"Yellow", TokenType.COLOR },
        {"Cyan", TokenType.COLOR },
        {"Grey", TokenType.COLOR },
        {"Orange", TokenType.COLOR },
        {"Pink", TokenType.COLOR },
        {"Purple", TokenType.COLOR },
        {"Circle", TokenType.SHAPE },

        {"Diamond", TokenType.SHAPE },
        {"Hexagon", TokenType.SHAPE },
        {"Square", TokenType.SHAPE },
        {"Start", TokenType.SHAPE },
        {"Triange", TokenType.SHAPE },
        {"Cross", TokenType.SHAPE },
        {"Moon", TokenType.SHAPE },
        {"Raindrop", TokenType.SHAPE },
        {"Kite", TokenType.SHAPE },
        {"Pentagon", TokenType.SHAPE },
        {"UpsideDownHouse", TokenType.SHAPE },

        {"True", TokenType.BOOL },
        {"False", TokenType.BOOL },

        {"Temp", TokenType.TEMP },
        {"HasExplicitMod", TokenType.EXLICIT_MOD_DECORATOR },
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
        _currentLine = 0;
        _currentCharacter = Peek();
        while (Peek() != '\0' || _currentIndex < _input.Length)
        {
            switch (_currentCharacter)
            {
                case '#':
                //line comment;
                {
                    while (_currentCharacter != '\n')
                    {
                        Advance();
                    }
                    Advance();
                }
                break;
                case '>':
                {
                    if (Peek(1) == '=')
                    {
                        result.Add(new Token() { type = TokenType.BOOL_OPERATOR, Line = _currentLine, Value = ">=" });
                        Advance();
                    }
                    else
                    {
                        result.Add(new Token() { type = TokenType.BOOL_OPERATOR, Line = _currentLine, Value = ">" });
                    }
                    Advance();
                }
                break;
                case '<':
                {
                    if (Peek(1) == '=')
                    {
                        result.Add(new Token() { type = TokenType.BOOL_OPERATOR, Line = _currentLine, Value = "<=" });
                        Advance();
                    }
                    else
                    {
                        result.Add(new Token() { type = TokenType.BOOL_OPERATOR, Line = _currentLine, Value="<" });
                    }
                    Advance();
                }
                //less
                break;
                case '=':
                {
                    if (Peek(1) == '=')
                    {
                        result.Add(new Token() { type = TokenType.BOOL_OPERATOR, Line = _currentLine , Value="=="});
                        Advance();
                    }
                    else
                    {
                        result.Add(new Token() { type = TokenType.BOOL_OPERATOR, Line = _currentLine, Value = "=" });
                    }
                    Advance();
                }
                //equals
                break;
                case '"':
                {
                    Advance();
                    List<char> word = [];
                    while (_currentCharacter != '"')
                    {
                        if (_currentCharacter == '\n')
                        {
                            throw new LexerError($"multiline string : {_currentLine}");
                        }
                        word.Add(_currentCharacter);
                        Advance();
                    }
                    result.Add(new Token { type = TokenType.STRING, Value = string.Join("", word), Line = _currentLine });
                    Advance();
                }
                break;
                case '!':
                {
                    if (Peek(1) == '=')
                    {
                        result.Add(new Token()
                        {
                            type = TokenType.BOOL_OPERATOR,
                            Line = _currentLine,
                            Value = "!="
                        });
                        Advance();
                        Advance();
                    }
                    else
                    {
                        throw new LexerError($"unknow character: {_currentLine}  at {_currentCharacter}");
                    }
                }
                break;
                case '\0':
                result.Add(new Token() { type = TokenType.EOF, Line = _currentLine });
                Advance();
                break;
                default:
                if (char.IsNumber(_currentCharacter))
                {
                    List<char> numbers = [_currentCharacter];
                    Advance();
                    while (char.IsNumber(_currentCharacter))
                    {
                        numbers.Add(_currentCharacter);
                        Advance();
                    }
                    string number = string.Join("", numbers);
                    result.Add(new Token() { type = TokenType.NUMBER, Value = number, Line = _currentLine });

                }
                else if (char.IsWhiteSpace(_currentCharacter))
                {
                    Advance();
                }
                else
                {
                    List<char> word = [_currentCharacter];
                    Advance();
                    while (char.IsLetter(_currentCharacter))
                    {
                        word.Add(_currentCharacter);
                        Advance();
                    }
                    string keyword = string.Join("", word);
                    if (_keywordsMap.ContainsKey(keyword))
                    {
                        result.Add(new Token() { type = _keywordsMap[keyword], Value = keyword, Line = _currentLine });
                    }
                    else
                    {
                        result.Add(new Token() { type = TokenType.STRING, Value = keyword, Line = _currentLine });
                    }
                }
                break;
            }
        }

        result.Add(new Token() { type = TokenType.EOF, Line = _currentLine });
        return result;
    }

    private void Advance()
    {
        _currentIndex++;
        if (_currentIndex + 1 >= _input.Length)
        {
            _currentCharacter = '\0';
            return;
        }

        if (_input[_currentIndex] == '\n')
        {
            _currentLine++;
        }
    
        
        _currentCharacter = _input[_currentIndex];
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
