using FilterDM.ViewModels.EditPage;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockDetailsViewModelTests
{
    [Test]
    public void NewRuleCommand_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = new(new(), new(), new(new()));
        RuleCreateRequestListener listener = new();

        sut.NewRuleCommand.Execute(sut);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Block, Is.EqualTo(sut));
    }
}
