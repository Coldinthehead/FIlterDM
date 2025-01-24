using FilterDM.Tests.Helpers;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Tests.EditorTests;
public class RuleEditorViewModelTests
{
    [Test]
    public void AddModifierClick_ShouldAddRuleModifier()
    {
        RuleDetailsViewModel testModel = WithEmptyModel();
        RuleEditorViewModel sut = new(testModel);
        AddModifierViewModel source = sut.AddModifiersList[0];

        source.AddMeCommand.Execute(null);

        Assert.That(testModel.Modifiers, Has.Count.EqualTo(2));
    }

    public static RuleDetailsViewModel WithEmptyModel()
    {
        return HelperFactory.GetRule(HelperFactory.GetBlock());

    }
}
