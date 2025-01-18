
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class FilterViewModelTests
{
    [Test]
    public void NewBlock_ShouldCreateEmptyBlock()
    {
        FilterViewModel sut = new FilterViewModel(new ItemTypeService(), new BlockTemplateService());

        sut.NewBlock();

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }


    [Test]
    public void NewBlock_ShouldApplyEmptyTemplateToNewBlock()
    {
        BlockTemplateService templateService = new();
        BlockModel empty = templateService.GetEmpty();
        FilterViewModel sut = new(new ItemTypeService(), templateService);
        sut.NewBlock();
        BlockDetailsViewModel newBlock = sut.Blocks.First();

        Assert.That(newBlock.SelectedTempalte, Is.EqualTo(empty.TemplateName));
        Assert.That(newBlock.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(newBlock.Priority, Is.EqualTo(empty.Priority));
        Assert.That(newBlock.Rules, Has.Count.EqualTo(empty.Rules.Count));
    }

    [Test]
    public void NewBlock_ShouldSetBlockTitleToGenericName_WhenBlocksEmpty()
    {
        FilterViewModel sut = new(new(), new());

        sut.NewBlock();

        Assert.That(sut.Blocks.First().Title, Is.EqualTo("Block"));
    }

    [Test]
    public void NewBlock_ShouldCreateBlockWithUniqueName()
    {
        FilterViewModel sut = new(new ItemTypeService(), new BlockTemplateService());
        sut.NewBlock();
        sut.NewBlock();
        BlockDetailsViewModel first = sut.Blocks.First();

        HashSet<string> titles = [.. sut.Blocks.Select(x => x.Title)];
        Assert.That(titles, Has.Count.EqualTo(sut.Blocks.Count));
    }

    [Test]
    public void NewBlock_ShouldRaiseBlockCreatedEvent()
    {
        FilterViewModel sut = new(new(), new());
        BlockCreatedListener listener = new();

        sut.NewBlock();
        BlockDetailsViewModel newBlock = sut.Blocks.First();

        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.EventModel, Is.Not.Null);
        Assert.That(listener.EventModel, Is.EqualTo(newBlock));
    }


    public class BlockCreatedListener : ObservableRecipient, IRecipient<BlockCreatedRequestEvent>
    {
        public bool Recieved = false;
        public BlockDetailsViewModel? EventModel;
        public BlockCreatedListener()
        {
            Messenger.Register<BlockCreatedRequestEvent>(this);
        }

        public void Receive(BlockCreatedRequestEvent message)
        {
            Recieved = true;
            EventModel = message.Value;
        }
    }
}
