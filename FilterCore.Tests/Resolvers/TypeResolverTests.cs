using FilterCore.Parser;

namespace FilterCore.Tests.Resolvers;
public class TypeResolverTests
{
    [Test]
    public void Resolve_ShouldRecognizeNumber()
    {
        TypeResolver sut = new();
        RuleNode input = new RuleNode();

        input.Parameters.AddRange([
            new Token() {type = TokenType.STRING, Value = "255",},
            new Token() {type = TokenType.STRING, Value = "13",},
            new Token() {type = TokenType.STRING, Value = "25",},
            
            ]);

        bool corrent = sut.Resolve(input);

        Assert.That(input.GetParameterMeta(0), Is.EqualTo(ParameterType.Number));
        Assert.That(input.GetParameterMeta(1), Is.EqualTo(ParameterType.Number));
        Assert.That(input.GetParameterMeta(2), Is.EqualTo(ParameterType.Number));
    }
}
