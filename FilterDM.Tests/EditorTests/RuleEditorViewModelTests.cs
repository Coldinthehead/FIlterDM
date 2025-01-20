using FilterDM.Repositories;
using FilterDM.Services;
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
        return new RuleDetailsViewModel(new(), new(new Services.ItemTypeService()), new(new RuleTemplateService(new RuleTemplateRepository())));

    }
}
