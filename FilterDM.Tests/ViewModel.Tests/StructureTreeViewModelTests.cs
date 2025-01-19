using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using NUnit.Framework.Internal;
using System.Collections.ObjectModel;

namespace FilterDM.Tests.ViewModel.Tests;
public class StructureTreeViewModelTests
{
    private FilterViewModel _filterVm;

    [SetUp]
    public void SetUp()
    {
        _filterVm = new(new(), new(new BlockTemplateRepository()), new());
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
    public void ShouldRaiseRuleSelectedEvent_WhenRuleSelected()
    {
        BlockDetailsViewModel block = _filterVm.Blocks.First();
        _filterVm.NewRule(block);
        RuleDetailsViewModel testRule = block.Rules.First();
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);
        RuleSelectListener listener = new();

        sut.Select(testRule);


        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.Selection, Is.EqualTo(testRule));
    }

    [Test]
    public void ShouldClearSelection_WhenBlockEditorClosedEvent()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);
        BlockDetailsViewModel block = _filterVm.Blocks.First();
        sut.Select(block);

        WeakReferenceMessenger.Default.Send(new EditorClosedEvent(new BlockEditorViewModel(block)));

        Assert.That(sut.SelectedNode , Is.Null);
    }

    [Test]
    public void ShouldChangeBlocks_WhenCollectionChangedEventRaised()
    {
        StructureTreeViewModel sut = new();

        ObservableCollection<BlockDetailsViewModel> next = new();

        WeakReferenceMessenger.Default.Send(new BlockCollectionInFilterChanged(next));

        Assert.That(sut.Blocks, Is.EqualTo(next));
    }

    [Test]
    public void SelectionShouldNotClear_WhenCollectionChanged()
    {
        StructureTreeViewModel sut = new();
        sut.SetBlocks(_filterVm.Blocks);
        BlockDetailsViewModel selectedBlock = _filterVm.Blocks.First();
        sut.Select(selectedBlock);

        _filterVm.SortBlocks();

        Assert.That(sut.SelectedNode, Is.EqualTo(selectedBlock));
    }
}
