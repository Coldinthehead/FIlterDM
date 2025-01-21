using FilterCore.Parser;

namespace FilterCore.Tests.Parser;
public class ParserTests
{

    [Test]
    public void Parse_ShouldParseEmptyTokensCorrect()
    {
        RuleParser sut = new();
        List<Token> input = [new Token() { type = TokenType.EOF }];
        List<Rule> result = sut.Parse(input);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_ShouldAddError()
    {
        RuleParser sut = new();
        List<Token> input = [new Token() { type = TokenType.MODIFIER_KEYWORD }, new Token() { type = TokenType.EOF }];

        List<Rule> result = sut.Parse(input);

        Assert.That(result, Is.Empty);
        Assert.That(sut.Errors, Has.Count.EqualTo(1));
    }

    [Test]
    public void Parse_ShouldParseKeywords()
    {
        RuleParser sut = new();
        List<Token> input = [new Token() { type = TokenType.RULE_START},
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.EOF }];

        List<Rule> result = sut.Parse(input);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(sut.Errors, Is.Empty);
    }

    [Test]
    public void Parse_ShouldRecover_WhemErrorInRuleStart()
    {
        RuleParser sut = new();
        List<Token> input = [
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.RULE_START},
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.EOF }
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

            new Token() { type = TokenType.RULE_START},
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.EOF }
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

            new Token() { type = TokenType.RULE_START},
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.STRING},
            new Token() { type = TokenType.EOF }
            ];

        List<Rule> result = sut.Parse(input);

        Assert.That(result.First().Nodes.First().Parameters, Has.Count.EqualTo(4));
    }

    [Test]
    public void Parse_ShouldHandleEmptyArguments()
    {
        RuleParser sut = new();
        List<Token> input = [

            new Token() { type = TokenType.RULE_START},
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.MODIFIER_KEYWORD },
            new Token() { type = TokenType.EOF }
            ];

        List<Rule> result = sut.Parse(input);

        Assert.That(result.First().Nodes, Has.Count.EqualTo(4));
    }
}
