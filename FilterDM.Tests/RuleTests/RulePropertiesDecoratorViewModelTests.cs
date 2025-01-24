using FilterDM.Tests.Helpers;
using FilterDM.Tests.ViewModel.Tests;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.RuleTests;
public class RulePropertiesDecoratorViewModelTests
{
    [Test]
    public void OnSortRules_ShouldRaiseSortRulesRequest()
    {
        RuleDetailsViewModel rule = HelperFactory.GetRule(HelperFactory.GetBlock());
        RulePropertiesDecoratorViewModel sut = rule.Properties;
        EventListener<SortRulesRequest, RuleDetailsViewModel> listener = new();
        sut.OnSortRules();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(rule));
    }
}
