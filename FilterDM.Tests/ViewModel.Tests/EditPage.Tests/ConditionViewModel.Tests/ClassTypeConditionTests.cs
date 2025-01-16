using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Conditions;
using FilterDM.ViewModels.EditPage.Helper;

namespace FilterDM.Tests.ViewModel.Tests.EditPage.Tests.ConditionViewModel.Tests;
internal class ClassTypeConditionTests
{
    [Test]
    [TestCase(true, ClassTypeState.Classes)]
    [TestCase(false, ClassTypeState.ItemNames)]
    public void ViewModelSetStateTo_WhenUsedInModel(bool modelState
        , ClassTypeState state)
    {
        ClassTypeConditionModel model = new()
        {
            UseClassNames = modelState
        };
        ClassTypeConditionViewModel sut = new(model);

        Assert.That(sut.State, Is.EqualTo(state));
    }

    [Test]
    public void ViewModel_CreateItem_WhenExistsInModel()
    {
        ClassTypeConditionModel model = new();
        model.AddName("Name1");
        ClassTypeConditionViewModel sut = new(model);

        Assert.That(sut.TypeRuleItems, Has.Count.EqualTo(model.Names.Count));
    }

    [Test]
    public void ViewModel_CreateItemWithSameData()
    {
        ClassTypeConditionModel model = new();
        model.AddName("Name1");
        ClassTypeConditionViewModel sut = new(model);

        ClassTypeViewModel vm = sut.TypeRuleItems
            .Where(x => x.Title.Equals("Name1")).FirstOrDefault();

        Assert.That(vm.Title, Is.EqualTo("Name1"));
    }

    [Test]
    public void ViewModel_UpdatesModel_WhenTypeDeleted()
    {
        ClassTypeConditionModel model = new();
        model.AddName("Name1");
        ClassTypeConditionViewModel sut = new(model);

        ClassTypeViewModel vm = sut.TypeRuleItems
            .Where(x => x.Title.Equals("Name1")).FirstOrDefault();

        vm.DeleteMeCommand.Execute(vm);

        Assert.That(model.Names, Is.Empty);
        Assert.That(model.Names, Has.Count.EqualTo(sut.TypeRuleItems.Count));
    }

    [Test]
    public void ViewModel_UpdatesModel_WhenTypeAdded()
    {
        ClassTypeConditionModel model = new();
        ClassTypeConditionViewModel sut = new(model);

        sut.TypeRuleInput = "Name1";
        sut.AddTypeClassCommand.Execute(null);

        Assert.That(model.Names, Has.Count.EqualTo(1));
        Assert.That(model.Names[0], Is.EqualTo("Name1"));
    }
}
