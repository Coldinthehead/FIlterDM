using FilterCore.Parser;

namespace FilterCore.Tests.Lexer;
public class LexerTests
{
    [Test]
    public void BuildTokens_ShouldInserEOFToken()
    {
        FilterLexer sut = new FilterLexer();
        string input = "";

        List<Token> result = sut.BuildTokens(input);

        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Last().type, Is.EqualTo(TokenType.EOF));
    }

    [Test]
    public void BuildTokens_ShouldNotFail_WhenInputEmpty()
    {
        FilterLexer sut = new FilterLexer();
        string input = "";

        List<Token> result = sut.BuildTokens(input);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public void BuildTokens_ShouldIgnoreSingleLineComments()
    {
        FilterLexer sut = new FilterLexer();
        string input = "# hello world";

        List<Token> result = sut.BuildTokens(input);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].type, Is.EqualTo(TokenType.EOF));
    }

    [Test]
    public void BuildTokens_ShouldParseStartRule()
    {
        FilterLexer sut = new FilterLexer();
        string input = "Show";

        List<Token> result = sut.BuildTokens(input);


        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.First().type, Is.EqualTo(TokenType.RULE_START));
    }


    [Test]
    public void BuildToken_ShouldIgnoreHalfLineComments()
    {
        FilterLexer sut = new FilterLexer();
        string input = "Show # sagsagsagsagasg \n \0";
        List<Token> result = sut.BuildTokens(input);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.First().type, Is.EqualTo(TokenType.RULE_START));
    }

    [Test]
    [TestCase("AreaLevel")]
    [TestCase("DropLevel")]
    [TestCase("ItemLevel")]
    [TestCase("Quality")]
    [TestCase("Sockets")]
    [TestCase("StackSize")]
    [TestCase("Height")]
    [TestCase("Width")]
    [TestCase("WaystoneTier")]
    [TestCase("BaseEnergyShield")]
    [TestCase("BaseArmour")]
    [TestCase("BaseEvasion")]
    [TestCase("Rarity")]
    [TestCase("Class")]
    [TestCase("BaseType")]
    [TestCase("Corrupted")]
    [TestCase("Mirrored")]
    [TestCase("AnyEnchantment")]
    [TestCase("DisableDropSound")]
    [TestCase("SetFontSize")]
    [TestCase("SetBorderColor")]
    [TestCase("SetTextColor")]
    [TestCase("SetBackgroundColor")]
    [TestCase("PlayAlertSound")]
    [TestCase("PlayAlertSoundPositional")]
    [TestCase("MinimapIcon")]
    [TestCase("PlayEffect")]
    [TestCase("HasExplicitMod")]
    public void BuildToken_ShouldRecognizeKeywords(string input)
    {
        FilterLexer sut = new();

        List<Token> result = sut.BuildTokens(input);

        Assert.That(result.First().type, Is.EqualTo(TokenType.MODIFIER_KEYWORD));
    }

    [Test]
    public void BuildToken_ShouldRecognizeQuotedString()
    {
        FilterLexer sut = new();
        string input = "\"Hello world\"";
        List<Token> result = sut.BuildTokens(input);

        Assert.That(result.First().type, Is.EqualTo(TokenType.STRING));
    }

    [Test]
    public void BuildToken_ShouldRecognizeSpaceSpaperatedValuesAsString()
    {
        FilterLexer sut = new();
        string input = "foo bar bazz";
        List<Token> result = sut.BuildTokens(input);

        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result.First().type, Is.EqualTo(TokenType.STRING));
    }

    [Test]
    public void BuildToken_ShouldRecogrnizeNumbersAsStrings()
    {
        FilterLexer sut = new();
        string input = "231 123 32 ";
        List<Token> result = sut.BuildTokens(input);

        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result.First().type, Is.EqualTo(TokenType.STRING));
    }

    [Test]
    [TestCase("=")]
    [TestCase("==")]
    [TestCase(">=")]
    [TestCase("<=")]
    [TestCase("<")]
    [TestCase(">")]
    public void BuildToken_ShouldRecogrnieBoolOperators(string input)
    {
        FilterLexer sut = new();
        List<Token> result = sut.BuildTokens(input);
        Assert.That(result.First().type, Is.EqualTo(TokenType.BOOL_OPERATOR));
    }

}
