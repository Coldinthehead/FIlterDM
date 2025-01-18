using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class StructureTreeViewModelTests
{
    private FilterViewModel _filterVm;

    [SetUp]
    public void SetUp()
    {
        _filterVm = new(new(), new(), new());
        _filterVm.NewBlock();
        _filterVm.NewBlock();
    }


    [Test]
    public void SetBlocks_ShouldReplicateBlocks()
    {
        StructureTreeViewModel sut = new();

        sut.SetBlocks(_filterVm.Blocks);

        Assert.That(sut.Blocks, Is.EqualTo(_filterVm.Blocks));
    }

    [Test]
    public void ShouldAddBlock_WhenNewBlockCreated()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);

        _filterVm.NewBlock();

        Assert.That(sut.Blocks, Has.Count.EqualTo(_filterVm.Blocks.Count));
    }

    [Test]
    public void ShouldSelectLastCreatedBlock()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);

        _filterVm.NewBlock();

        Assert.That(sut.SelectedNode, Is.EqualTo(_filterVm.Blocks.Last()));
    }

    [Test]
    public void ShouldClearSelection_WhenSelectedBlockDeleted()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);
        sut.Select(_filterVm.Blocks.First());

        _filterVm.DeleteBlock(_filterVm.Blocks.First());

        Assert.That(sut.SelectedNode, Is.Null);
    }

    [Test]
    public void ShouldChangeBlockCollection_WhenCollectionChangedInFilter()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);


        _filterVm.SetModel(new FilterModel());

        Assert.That(sut.Blocks, Is.EqualTo(_filterVm.Blocks));
    }

    [Test]
    public void Select_ShouldRaiseEvent_WhenSelectionIsNotNull()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);
        SelectLisener listener = new();

        sut.Select(_filterVm.Blocks.First());

        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.Selection, Is.EqualTo(_filterVm.Blocks.First()));
    }

    [Test]
    public void ShouldDeselect_WhenBlockEdtorClosedRaisedWithSelectedBlock()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);
        BlockDetailsViewModel testBlock = _filterVm.Blocks.First();
        sut.Select(testBlock);

        WeakReferenceMessenger.Default.Send(new BlockEditorClosed(new BlockEditorViewModel(testBlock)));

        Assert.That(sut.SelectedNode, Is.Null);
    }

    public class SelectLisener : ObservableRecipient
        , IRecipient<BlockSelectedInTree>
    {
        public bool Recieved = false;
        public BlockDetailsViewModel Selection;

        public SelectLisener()
        {
            Messenger.Register(this);
        }

        public void Receive(BlockSelectedInTree message)
        {
            Recieved = true;
            Selection = message.Value;
        }
    }
}
