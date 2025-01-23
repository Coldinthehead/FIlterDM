using FilterCore.Parser;
using System.Security;

namespace FilterCore.Tests.Parser;
public class ParserTests
{

    [Test]
    public void Parse_ShouldParseEmptyTokensCorrect()
    {
        RuleParser sut = new();
        List<Token> input = [new Token("\0") { type = TokenType.EOF }];
        List<Rule> result = sut.Parse(input);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_ShouldAddError()
    {
        RuleParser sut = new();
        List<Token> input = [new Token(null) { type = TokenType.MODIFIER_KEYWORD }, new Token("\0") { type = TokenType.EOF }];

        List<Rule> result = sut.Parse(input);

        Assert.That(result, Is.Empty);
        Assert.That(sut.Errors, Has.Count.EqualTo(1));
    }

    [Test]
    public void Parse_ShouldParseKeywords()
    {
        RuleParser sut = new();
        List<Token> input = [new Token("Show") { type = TokenType.RULE_START},
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token("\0") { type = TokenType.EOF }];

        List<Rule> result = sut.Parse(input);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(sut.Errors, Is.Empty);
    }

    [Test]
    public void Parse_ShouldRecover_WhemErrorInRuleStart()
    {
        RuleParser sut = new();
        List<Token> input = [
            new Token("test") { type = TokenType.STRING},
            new Token("Hide") { type = TokenType.RULE_START},
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token("\0") { type = TokenType.EOF }
            ];

        List<Rule> result = sut.Parse(input);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(sut.Errors, Has.Count.EqualTo(1));
    }

    [Test]
    public void Parse_ShouldRcover_WhenErrorInModifierParameter()
    {
        RuleParser sut = new();
        List<Token> input = [

            new Token("Show") { type = TokenType.RULE_START},
            new Token(null) { type = TokenType.STRING},
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token(null) { type = TokenType.STRING},
            new Token("\0") { type = TokenType.EOF }
            ];

        List<Rule> result = sut.Parse(input);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(sut.Errors, Has.Count.EqualTo(1));
    }

    [Test]
    public void Parse_ShouldHandleMultipleArguments()
    {
        RuleParser sut = new();
        List<Token> input = [

            new Token("Show") { type = TokenType.RULE_START},
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token(null) { type = TokenType.STRING},
            new Token(null) { type = TokenType.STRING},
            new Token(null) { type = TokenType.STRING},
            new Token(null) { type = TokenType.STRING},
            new Token("\0") { type = TokenType.EOF }
            ];

        List<Rule> result = sut.Parse(input);

        Assert.That(result.First().Nodes.First().Parameters, Has.Count.EqualTo(4));
    }

    [Test]
    public void Parse_ShouldHandleEmptyArguments()
    {
        RuleParser sut = new();
        List<Token> input = [

            new Token("Show") { type = TokenType.RULE_START},
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token(null) { type = TokenType.MODIFIER_KEYWORD },
            new Token("\0") { type = TokenType.EOF }
            ];

        List<Rule> result = sut.Parse(input);

        Assert.That(result.First().Nodes, Has.Count.EqualTo(4));
    }

    [Test]
    public void ShouldParseEdgeCaseCorrect()
    {
        FilterLexer lexer = new();
        string input = @"
        Show # block : T0
        BaseType  ""Mirror of Kalandra"" ""Divine Orb"" ""Albino Rhoa Feather"" ""Orb of Transmutation""
        SetFontSize 45
        SetBackgroundColor 255 255 255 255
        SetTextColor 255 0 0 255
        SetBorderColor 255 0 0 255
        MinimapIcon 0 Red Diamond
        PlayEffect Red
        PlayAlertSound 1 300
";

        RuleParser sut = new();

        List<Rule> rules = sut.Parse(lexer.BuildTokens(input));

        Rule result = rules[0];

        Assert.That(rules, Has.Count.EqualTo(1));
        Assert.That(sut.Errors, Is.Empty);
        Assert.That(result.Nodes , Has.Count.EqualTo(8));
    }
}
