﻿using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;

namespace FilterDM.Tests.ViewModel.Tests;
public class EditorPanelViewModelTests
{
    [Test]
    public void Clear_ShouldDeleteAllEditors()
    {
        EditorPanelViewModel sut = new();
        sut.AddBlock(new(new(new(new BlockTemplateRepository())), new(new())));

        sut.Clear();

        Assert.That(sut.Items, Is.Empty);
    }
    [Test]
    public void AddBlock_ShouldCreateBlockEditor_WhenEditorNotExists()
    {
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock =  new(new(new(new BlockTemplateRepository())), new(new()));

        sut.AddBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.First().IsPartOf(testBlock), Is.True);
    }

    [Test]
    public void AddBlock_ShouldNotCreateBlockEditor_WhenEditorExists()
    {
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = new(new(new(new BlockTemplateRepository())), new(new()));
        sut.AddBlock(testBlock);

        sut.AddBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
    }

    [Test]
    public void ShouldOpenEditor_WhenBlockCreated()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel fitlerVm = new(new(), new(new BlockTemplateRepository()), new(new RuleTemplateRepository()));

        fitlerVm.NewBlock();
        BlockDetailsViewModel testBlock = fitlerVm.GetBlocks().First();

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldCloseEditor_WhenBlockDeleted()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel fitlerVm = new(new(), new(new BlockTemplateRepository()), new(new RuleTemplateRepository()));
        fitlerVm.NewBlock();
        fitlerVm.NewBlock();

        BlockDetailsViewModel testBlock = fitlerVm.GetBlocks().First();
        fitlerVm.DeleteBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x=>x.IsPartOf(testBlock)).First(), Is.False);
    }

    [Test]
    public void ShouldOpenTab_WhenBlockSelectedInTree()
    {
        FilterViewModel fitlerVm = new(new(), new(new BlockTemplateRepository()), new(new RuleTemplateRepository()));
        fitlerVm.NewBlock();
        fitlerVm.NewBlock();
        StructureTreeViewModel tree = new();
        tree.SetBlocks(fitlerVm.GetBlocks());
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = fitlerVm.GetBlocks().First();

        tree.Select(testBlock);

        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldOpenEditor_WhenRuleSelectedEvent()
    {
        FilterViewModel fitlerVm = new(new(), new(new BlockTemplateRepository()),new(new RuleTemplateRepository()));
        fitlerVm.NewBlock();
        BlockDetailsViewModel block = fitlerVm.GetBlocks().First();
        fitlerVm.NewRule(block);
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
        FilterViewModel fitlerVm = new(new(), new(new BlockTemplateRepository()), new(new RuleTemplateRepository()));
        EditorPanelViewModel sut = new();
        fitlerVm.NewBlock();
        EditorBaseViewModel editor = sut.Items.First();
        
        WeakReferenceMessenger.Default.Send(new EditorClosedEvent(editor));

        Assert.That(sut.Items.Contains(editor), Is.False);
        Assert.That(sut.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public void ShouldCloseOpenedRules_WhenBlockTempalteChanged()
    {
        FilterViewModel filterVm = new(new(), new(new BlockTemplateRepository()), new(new RuleTemplateRepository()));
        EditorPanelViewModel sut = new();
        filterVm.NewBlock();
        BlockDetailsViewModel testBlock = filterVm.GetBlocks().First();
        filterVm.NewRule(testBlock);
        filterVm.NewRule(testBlock);
        filterVm.NewRule(testBlock);
        foreach (var rule in testBlock.Rules)
        {
            sut.AddRule(rule);
        }
        BlockModel empty = new BlockTemplateRepository().GetEmpty();

        filterVm.ResetBlockTemplate(testBlock, empty);

        Assert.That(sut.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public void ShouldCloseEditor_WhenRuleDeleted()
    {
        EditorPanelViewModel sut = new();
        RuleDetailsViewModel testModel = new(new(), null, new RuleTemplateManager(new RuleTemplateService(new RuleTemplateRepository())), new());
        sut.AddRule(testModel);

        WeakReferenceMessenger.Default.Send(new RuleDeleteEvent(testModel));
        
        Assert.That(sut.Items, Has.Count.EqualTo(0));   
    }
}
