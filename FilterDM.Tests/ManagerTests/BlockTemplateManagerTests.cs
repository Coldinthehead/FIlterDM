using FilterDM.Repositories;
using FilterDM.Tests.ViewModel.Tests;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage;
using FilterDM.Managers;
using FilterDM.Services;
using FilterDM.Models;

namespace FilterDM.Tests.ManagerTests;
public class BlockTemplateManagerTests
{
    [Test]
    public void SetTemplate_ShouldRaiseEvent()
    {
        BlockTemplateManager sut = new(new Services.BlockTemplateService(new BlockTemplateRepository()));
        EventListener<ResetTemplateRequest, TemplateChangeDetils> listener = new();
        BlockDetailsViewModel testBlock = new(sut, new(new()));
        testBlock.AddRule(new RuleDetailsViewModel(new(), new(new()), new ViewModels.EditPage.Managers.RuleTemplateManager(new RuleTemplateService(new RuleTemplateRepository())));
        BlockModel template = new BlockTemplateRepository().GetEmpty();

        sut.SetTempalte(testBlock, template);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload.Template, Is.EqualTo(template));
        Assert.That(listener.Playload.Block, Is.EqualTo(testBlock));
    }
}
