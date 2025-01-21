﻿
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;

namespace FilterDM.Tests.ViewModel.Tests;
public class RuleDetailsViewModelTests
{
    [Test]
    public void OnDeleteConfirmed_ShouldRaiseRuleDeleteRequest()
    {
        RuleDetailsViewModel sut = new(new(), null, null);
        EventListener<DeleteRuleRequest, RuleDetailsViewModel> listener = new();
        sut.OnDeleteConfirmed();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(sut));
    }

    [Test]
    public void GetModel_ShouldSetProperties()
    {
        RuleDetailsViewModel sut = new(new(), new(new ItemTypeService()), new RuleTemplateManager(new RuleTemplateService(new RuleTemplateRepository())));

        RuleModel testModel = sut.GetModel();

        Assert.That(testModel, Is.Not.Null);
        Assert.That(testModel.Title, Is.Not.Null);
        Assert.That(testModel.Enabled, Is.EqualTo(sut.Properties.Enabled));
        Assert.That(testModel.Priority, Is.EqualTo(sut.Properties.Priority));
        Assert.That(testModel.Show, Is.EqualTo(sut.Properties.Show));
    }
}
