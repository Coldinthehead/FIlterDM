using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockDetailsViewModelTests
{
    [Test]
    public void NewRuleCommand_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = new(new(new(new BlockTemplateRepository())), new(new()));
        RuleCreateRequestListener listener = new();

        sut.NewRuleCommand.Execute(sut);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Block, Is.EqualTo(sut));
    }

    [Test]
    public void OnDeleteConfirmed_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = new(new(new(new BlockTemplateRepository())), new(new()) );
        EventListener<DeleteBlockRequest,BlockDetailsViewModel> listener = new();

        sut.OnDeleteConfirmed();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(sut));
    }

    [Test]
    public void SetModel_ShouldChangeBlockCorrect()
    {
        BlockDetailsViewModel sut = new(new(new(new BlockTemplateRepository())), new(new()));
        BlockModel empty = new BlockTemplateRepository().GetEmpty();

        sut.SetModel(empty);

        Assert.That(sut.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(sut.Priority, Is.EqualTo(empty.Priority));
    }
}
