﻿using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.Tests.Helpers;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockDetailsViewModelTests
{
    [Test]
    public void NewRuleCommand_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = HelperFactory.GetBlock();
        RuleCreateRequestListener listener = new();

        sut.NewRuleCommand.Execute(sut);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Block, Is.EqualTo(sut));
    }

    [Test]
    public void OnDeleteConfirmed_ShouldRaiseEvent()
    {
        BlockDetailsViewModel sut = HelperFactory.GetBlock();
        EventListener<DeleteBlockRequest,BlockDetailsViewModel> listener = new();

        sut.OnDeleteConfirmed();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(sut));
    }

    [Test]
    public void SetModel_ShouldChangeBlockCorrect()
    {
        BlockDetailsViewModel sut = HelperFactory.GetBlock();
        BlockModel empty = new BlockTemplateRepository( new PersistentDataService()).GetEmpty();

        sut.SetModel(empty);

        Assert.That(sut.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(sut.Priority, Is.EqualTo(empty.Priority));
        Assert.That(sut.SelectedTemplate.TemplateName, Is.EqualTo(empty.TemplateName));
    }

    [Test]
    public void GetModel_ShouldSetFileds()
    {
        BlockDetailsViewModel sut = HelperFactory.GetBlock();

        BlockModel testModel = sut.GetModel();

        Assert.That(testModel, Is.Not.Null);
        Assert.That(testModel.Enabled, Is.EqualTo(sut.Enabled));
        Assert.That(testModel.Priority, Is.EqualTo(sut.Priority));
        Assert.That(testModel.TemplateName, Is.EqualTo(sut.SelectedTemplate.Title));
        Assert.That(testModel.UseBlockTypeScope, Is.EqualTo(sut.UseScopeNames));
    }

    [Test]
    public void GetModel_ShouldSetModelTitleAsEmpty_WhenTitleIsNull()
    {
        BlockDetailsViewModel sut = HelperFactory.GetBlock();
        sut.Title = null;
        BlockModel testModel = sut.GetModel();

        Assert.That(testModel.Title, Is.Not.Null);
        Assert.That(testModel.Title, Is.EqualTo("Empty"));
    }
}
