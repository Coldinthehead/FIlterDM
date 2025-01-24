using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.Tests.Helpers;
using FilterDM.Tests.ViewModel.Tests;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ManagerTests;
public class RuleTemplateManagerTests
{
    [Test]
    public void SetTemplate_ShouldRaiseResetRuleTemplateRequest()
    {
        RuleTemplateManager sut = new(new RuleTemplateService(new RuleTemplateRepository()));
        RuleDetailsViewModel testRule = HelperFactory.GetRule(HelperFactory.GetBlock());
        EventListener<ResetRuleTemplateRequest, ResetRuleTemplateDetails> listener = new();
        RuleModel template = new RuleTemplateRepository().GetEmpty();

        sut.SetTemplate(testRule, template);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload.Rule, Is.EqualTo(testRule));
        Assert.That(listener.Playload.Template, Is.EqualTo(template));
    }
}
