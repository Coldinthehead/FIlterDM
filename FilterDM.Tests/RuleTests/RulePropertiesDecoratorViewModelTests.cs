using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.Tests.ViewModel.Tests;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;

namespace FilterDM.Tests.RuleTests;
public class RulePropertiesDecoratorViewModelTests
{
    [Test]
    public void OnSortRules_ShouldRaiseSortRulesRequest()
    {
        RuleDetailsViewModel rule = new RuleDetailsViewModel(new()
            , new(new Services.ItemTypeService())
            , new(new RuleTemplateService(new RuleTemplateRepository())), new(), new(), new());
        RulePropertiesDecoratorViewModel sut = rule.Properties;
        EventListener<SortRulesRequest, RuleDetailsViewModel> listener = new();
        sut.OnSortRules();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(rule));
    }
}
