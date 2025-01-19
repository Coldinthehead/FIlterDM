using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

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

    [Test]
    public void OnDeleteConfirmed_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = new(new(), new(), new(new()) );
        EventListener<DeleteBlockRequest,BlockDetailsViewModel> listener = new();

        sut.OnDeleteConfirmed();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(sut));
    }

    [Test]
    public void OnTemplateResetConfirmed_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = new(new(), new(), new(new()));

        EventListener<ResetTemplateRequest, TemplateChangeDetils> listener = new();

        sut.OnTemplateResetConfirmed();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload.Block, Is.EqualTo(sut));
        Assert.That(listener.Playload.TempalteName, Is.EqualTo(sut.SelectedTemplate));
    }
}
