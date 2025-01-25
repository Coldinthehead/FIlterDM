using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.Tests.Helpers;
using FilterDM.ViewModels;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;


namespace FilterDM.Tests.ViewModel.Tests;
public class EditorPanelViewModelTests
{
    [Test]
    public void Clear_ShouldDeleteAllEditors()
    {
        EditorPanelViewModel sut = new();
        sut.AddBlock(HelperFactory.GetBlock());

        sut.Clear();

        Assert.That(sut.Items, Is.Empty);
    }
    [Test]
    public void AddBlock_ShouldCreateBlockEditor_WhenEditorNotExists()
    {
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = HelperFactory.GetBlock();

        sut.AddBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.First().IsPartOf(testBlock), Is.True);
    }

    [Test]
    public void AddBlock_ShouldNotCreateBlockEditor_WhenEditorExists()
    {
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = HelperFactory.GetBlock();
        sut.AddBlock(testBlock);

        sut.AddBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
    }

    [Test]
    public void ShouldOpenEditor_WhenBlockCreated()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel vm = HelperFactory.GetFilter();
        vm.NewBlock();
        BlockDetailsViewModel testBlock = vm.GetBlocks().First();

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldCloseEditor_WhenBlockDeleted()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel vm = HelperFactory.GetFilter();
        vm.NewBlock();
        vm.NewBlock();

        BlockDetailsViewModel testBlock = vm.GetBlocks().First();
        vm.DeleteBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x=>x.IsPartOf(testBlock)).First(), Is.False);
    }

    [Test]
    public void ShouldOpenTab_WhenBlockSelectedInTree()
    {
        FilterViewModel vm = HelperFactory.GetFilter();
        vm.NewBlock();
        vm.NewBlock();
        StructureTreeViewModel tree = new();
        tree.SetBlocks(vm.GetBlocks());
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = vm.GetBlocks().First();

        tree.Select(testBlock);

        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldOpenEditor_WhenRuleSelectedEvent()
    {
        FilterViewModel vm = HelperFactory.GetFilter();
        vm.NewBlock();
        BlockDetailsViewModel block = vm.GetBlocks().First();
        vm.NewRule(block);
        RuleDetailsViewModel testRule = block.Rules.First();
        EditorPanelViewModel sut = new();

        WeakReferenceMessenger.Default.Send(new RuleSelectedInTree(testRule));

        RuleEditorViewModel editor = sut.Items.First() as RuleEditorViewModel;
        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(editor, Is.Not.Null);
        Assert.That(editor.Rule, Is.EqualTo(testRule));
    }

    [Test]
    public void ShouldCloseBlockEditor_WhenEventRaised()
    {
        FilterViewModel vm = HelperFactory.GetFilter();
        EditorPanelViewModel sut = new();
        vm.NewBlock();
        EditorBaseViewModel editor = sut.Items.First();
        
        WeakReferenceMessenger.Default.Send(new EditorClosedEvent(editor));

        Assert.That(sut.Items.Contains(editor), Is.False);
        Assert.That(sut.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public void ShouldCloseOpenedRules_WhenBlockTempalteChanged()
    {
        FilterViewModel vm = HelperFactory.GetFilter();
        EditorPanelViewModel sut = new();
        vm.NewBlock();
        BlockDetailsViewModel testBlock = vm.GetBlocks().First();
        vm.NewRule(testBlock);
        vm.NewRule(testBlock);
        vm.NewRule(testBlock);
        foreach (var rule in testBlock.Rules)
        {
            sut.AddRule(rule);
        }
        BlockModel empty = new BlockTemplateRepository(new PersistentDataService()).GetEmpty();

        vm.ResetBlockTemplate(testBlock, empty);

        Assert.That(sut.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public void ShouldCloseEditor_WhenRuleDeleted()
    {
        EditorPanelViewModel sut = new();
        RuleDetailsViewModel testModel = HelperFactory.GetRule(HelperFactory.GetBlock());
        sut.AddRule(testModel);

        WeakReferenceMessenger.Default.Send(new RuleDeleteEvent(testModel));
        
        Assert.That(sut.Items, Has.Count.EqualTo(0));   
    }
}
