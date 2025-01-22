using System.Security.Cryptography;

namespace FilterCore.Parser;

public readonly struct LineDetails
{
    public readonly string Line;
    public readonly int Number;

    public LineDetails(string line, int number)
    {
        Line = line;
        Number = number;
    }
}

public class FilterLexer
{
    private int _currentIndex = 0;
    private string _input;

    private int _currentLine;

    private LineParser _lineParser = new();

    public List<Token> BuildTokens(string input)
    {
        _input = input;
        _currentIndex = 0;

        List<LineDetails> cleaned = [];

        string[] lines = input.Split("\n");
        int index = 0;
        foreach (string line in lines)
        {
            string? data = line.Split("#").FirstOrDefault();
            if (data != null)
            {
                data = data.Replace("\r", "").Replace("\t", "");
                if (data.Length > 0)
                {
                    cleaned.Add(new LineDetails(data, index));
                }
            }

            index++;
        }
        foreach (var item in cleaned)
        {
            Console.WriteLine($"{item.Number} : {item.Line}");
        }
        return Parse(cleaned);
    }

    private List<Token> Parse(List<LineDetails> lines)
    {
        List<Token> result = [];
        foreach (LineDetails details in lines)
        {
            result.AddRange(_lineParser.Parse(details));
        }
        result.Add(new Token("null")
        {
            type = TokenType.EOF,
        });
        return result;
    }
}

public class LineParser
{
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

    private int _i = 0;

    private string _input;

    public List<Token> Parse(LineDetails details)
    {
        _i = 0;
        if (details.Line.Length == 0)
        {
            return [];
        }
        else
        {
            _input = details.Line;
            List<Token> res = [];
            while (Peek() != '\0')
            {
                switch (Peek())
                {
                    case ' ':
                    {
                        Advance();
                    }
                    break;
                    case '>':
                    case '<':
                    case '=':
                    {
                        if (Peek(1) == '=')
                        {
                            Token t = new Token($"{Peek()}=")
                            {
                                type = TokenType.BOOL_OPERATOR,
                                Line = details.Number,
                            };
                            res.Add(t);
                            Advance();
                            Advance();
                        }
                        else
                        {
                            Token t = new Token($"{Peek()}")
                            {
                                type = TokenType.BOOL_OPERATOR,
                                Line = details.Number,
                            };
                            res.Add(t);
                            Advance();
                        }
                        Advance();
                    }
                    break;
                    case '"':
                    {
                        Advance();
                        List<char> chars = [];
                        while (Peek() != '"')
                        {
                            if (Peek() == '\0')
                            {
                                throw new LexerError($"Unterminated string at {details.Line} : {_i}");
                            }
                            chars.Add(Peek());
                            Advance();
                        }
                        Token t = new(string.Join("", chars))
                        {
                            type = TokenType.STRING,
                            Line = details.Number,
                        };
                        res.Add(t);
                        Advance();
                    }
                    break;
                    default:
                    {
                        if (char.IsDigit(Peek()))
                        {
                            List<char> chars = [];
                            while (char.IsDigit(Peek()))
                            {
                                chars.Add(Peek());
                                Advance();
                            }
                            Token t = new(string.Join("", chars))
                            {
                                type = TokenType.STRING,
                                Line = details.Number,
                            };
                            res.Add(t);
                        }
                        else if (char.IsLetter(Peek()))
                        {
                            List<char> chars = [];
                            while (char.IsLetter(Peek()))
                            {
                                chars.Add(Peek());
                                Advance();
                            }
                            string word = string.Join("", chars);
                            if (_keywordsMap.ContainsKey(word))
                            {
                                Token t = new(word)
                                {
                                    type = _keywordsMap[word],
                                    Line = details.Number,
                                };
                                res.Add(t);
                            }
                            else
                            {
                                Token t = new(word)
                                {
                                    type = TokenType.STRING,
                                    Line = details.Number,
                                };
                                res.Add(t);
                            }
                        }
                        else
                        {
                            throw new LexerError($"Unknown character at {details.Line} : {_i}");
                        }
                    }
                    break;
                }
            }


            return res;
        }
    }

    private char Peek(int ahead = 0)
    {
        if (_i + ahead >= _input.Length)
        {
            return '\0';
        }
        else
        {
            return _input[_i + ahead];
        }
    }

    private void Advance()
    {
        _i++;
    }
}