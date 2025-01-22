using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.Tests.ViewModel.Tests;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;

namespace FilterDM.Tests.ManagerTests;
public class RuleTemplateManagerTests
{
    [Test]
    public void SetTemplate_ShouldRaiseResetRuleTemplateRequest()
    {
        RuleTemplateManager sut = new(new RuleTemplateService(new RuleTemplateRepository()));
        RuleDetailsViewModel testRule = new(new RuleParentManager(), new Managers.TypeScopeManager(new ItemTypeService()), sut, new());
        EventListener<ResetRuleTemplateRequest, ResetRuleTemplateDetails> listener = new();
        RuleModel template = new RuleTemplateRepository().GetEmpty();

        sut.SetTemplate(testRule, template);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload.Rule, Is.EqualTo(testRule));
        Assert.That(listener.Playload.Template, Is.EqualTo(template));
    }
}
