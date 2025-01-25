using FilterDM.Repositories;
using FilterDM.Tests.ViewModel.Tests;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Tests.Helpers;
using FilterDM.Services;

namespace FilterDM.Tests.ManagerTests;
public class BlockTemplateManagerTests
{
    [Test]
    public void SetTemplate_ShouldRaiseEvent()
    {
        BlockTemplateManager sut = new(new Services.BlockTemplateService(new BlockTemplateRepository(new PersistentDataService())));
        EventListener<ResetTemplateRequest, TemplateChangeDetils> listener = new();
        BlockDetailsViewModel testBlock = HelperFactory.GetBlock();
        testBlock.AddRule(HelperFactory.GetRule(testBlock));
        BlockModel template = new BlockTemplateRepository(new PersistentDataService()).GetEmpty();

        sut.SetTempalte(testBlock, template);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload.Template, Is.EqualTo(template));
        Assert.That(listener.Playload.Block, Is.EqualTo(testBlock));
    }
}
