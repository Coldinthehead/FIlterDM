using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class FilterViewModelTests
{

    public static FilterViewModel Build()
    {
        return new FilterViewModel(new ItemTypeService(), new(new BlockTemplateRepository()), new RuleTemplateService());
    }

    [Test]
    public void NewBlock_ShouldCreateEmptyBlock()
    {
        FilterViewModel sut = Build();

        sut.NewBlock();

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }


    [Test]
    public void NewBlock_ShouldApplyEmptyTemplateToNewBlock()
    {
        BlockTemplateRepository templateService = new();
        BlockModel empty = templateService.GetEmpty();
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel newBlock = sut.Blocks.First();

        Assert.That(newBlock.SelectedTemplate, Is.EqualTo(empty.TemplateName));
        Assert.That(newBlock.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(newBlock.Priority, Is.EqualTo(empty.Priority));
        Assert.That(newBlock.Rules, Has.Count.EqualTo(empty.Rules.Count));
    }

    [Test]
    public void NewBlock_ShouldSetBlockTitleToGenericName_WhenBlocksEmpty()
    {
        FilterViewModel sut = Build();

        sut.NewBlock();

        Assert.That(sut.Blocks.First().Title, Is.EqualTo("Block"));
    }

    [Test]
    public void NewBlock_ShouldCreateBlockWithUniqueName()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        sut.NewBlock();
        BlockDetailsViewModel first = sut.Blocks.First();

        HashSet<string> titles = [.. sut.Blocks.Select(x => x.Title)];
        Assert.That(titles, Has.Count.EqualTo(sut.Blocks.Count));
    }

    [Test]
    public void NewBlock_ShouldRaiseBlockCreatedEvent()
    {
        FilterViewModel sut = Build();   
        BlockCreatedListener listener = new();

        sut.NewBlock();
        BlockDetailsViewModel newBlock = sut.Blocks.First();

        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.EventModel, Is.Not.Null);
        Assert.That(listener.EventModel, Is.EqualTo(newBlock));
    }

    [Test]
    public void ShouldCreateNewBlock_WhenRequestSended()
    {
        FilterViewModel sut = Build();

        WeakReferenceMessenger.Default.Send(new CreateBlockRequest(null));

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }

    [Test]
    public void SetModel_ShouldReplicateBlocks()
    {
        FilterViewModel sut = Build();
        FilterModel testModel = new FilterModel()
        {
            Name = "Hello",
            Blocks = new()
        };
        testModel.AddBlock(new BlockModel()
        {
            Title = "Hello1",
            Priority = 1,
            Enabled = true,
        });
        testModel.AddBlock(new BlockModel()
        {
            Title = "Hello2",
            Priority = 125,
            Enabled = false,
        });


        sut.SetModel(testModel);

        Assert.That(sut.Blocks.Count, Is.EqualTo(testModel.Blocks.Count));
        Assert.That(ModelMatchViewModel(testModel, sut), Is.True);
    }

    [Test]
    public void SetModel_ShouldRaiseBlocksCollectionChanged()
    {
        FilterViewModel sut = Build();
        BlockCollectionChangedListener listener = new();
        FilterModel testModel = new FilterModel()
        {
            Name = "Hello",
            Blocks = new()
        };
        testModel.AddBlock(new BlockModel()
        {
            Title = "Hello1",
            Priority = 1,
            Enabled = true,
        });
        testModel.AddBlock(new BlockModel()
        {
            Title = "Hello2",
            Priority = 125,
            Enabled = false,
        });

        sut.SetModel(testModel);

        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.Blocks, Is.EqualTo(sut.Blocks));
    }

    [Test]
    public void ShouldDeleteBlock_WhenDeleteRequestRaised()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();

        WeakReferenceMessenger.Default.Send(new DeleteBlockRequest(block));

        Assert.That(sut.Blocks, Does.Not.Contain(block));
    }

    [Test]
    public void DeleteBlock_ShouldRaiseBlockDeletedEvent()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();
        BlockDeletedListener listener = new();

        sut.DeleteBlock(block);

        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.Block, Is.EqualTo(block));
    }

    [Test]
    public void NewRule_ShouldUpdateParentWithNewRule()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();

        sut.NewRule(block);

        Assert.That(block.Rules, Has.Count.EqualTo(1));
    }

    [Test]
    public void NewRule_ShouldSetEmptyTemplate()
    {
        RuleTemplateService service = new();
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();

        sut.NewRule(block);
        RuleDetailsViewModel rule = block.Rules.First();

        RuleModel template = service.BuildEmpty();
        Assert.That(rule.Properties.Enabled, Is.EqualTo(template.Enabled));
        Assert.That(rule.Properties.Priority, Is.EqualTo(template.Priority));
        Assert.That(rule.Properties.Show, Is.EqualTo(template.Show));
    }

    [Test]
    public void NewRule_ShouldRaiseRuleCreatedEvent()
    {
        RuleTemplateService service = new();
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();
        RuleCreatedListener listener = new();

        sut.NewRule(block);
        RuleDetailsViewModel rule = block.Rules.First();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Rule, Is.EqualTo(rule));
    }

    [Test]
    public void ShouldSortBlocks_OnSortBlockRequestEvent()
    {
        RuleTemplateService service = new();
        FilterViewModel sut = Build();
        sut.NewBlock();
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();
        block.Priority = 100;

        WeakReferenceMessenger.Default.Send(new SortBlocksRequest(block));

        Assert.That(sut.Blocks.IndexOf(block), Is.EqualTo(1));
    }

    [Test]
    public void SortBlocks_ShouldRaiseBlocksChanged()
    {
        RuleTemplateService service = new();
        FilterViewModel sut = Build();
        sut.NewBlock();
        sut.NewBlock();
        NewBlocksListener listener = new();

        sut.SortBlocks();

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Blocks, Is.EqualTo(sut.Blocks));
    }

    [Test]
    public void ShouldCreateNewRule_OnCreateRuleEvent()
    {
        var messenger = new WeakReferenceMessenger();
        FilterViewModel sut = new(messenger);
        sut.NewBlock();
        BlockDetailsViewModel testBlock = sut.Blocks.First();

        messenger.Send(new CreateRuleRequest(testBlock));

        Assert.That(testBlock.Rules, Has.Count.EqualTo(1));
    }

    [Test]
    public void ShouldChangeBlockTemplate_OnChangeTemplateRequest()
    {
        BlockModel empty = new BlockTemplateRepository().GetEmpty();
        var messenger = new WeakReferenceMessenger();
        FilterViewModel sut = new(messenger);
        sut.NewBlock();
        BlockDetailsViewModel testBlock = sut.Blocks.First();
        sut.NewRule(testBlock);
        sut.NewRule(testBlock);
        sut.NewRule(testBlock);

        messenger.Send(new ResetTemplateRequest(new TemplateChangeDetils(testBlock, empty.TemplateName)));


        Assert.That(testBlock.Rules, Has.Count.EqualTo(0));
        Assert.That(testBlock.Priority, Is.EqualTo(empty.Priority));
        Assert.That(testBlock.Enabled, Is.EqualTo(empty.Enabled));
    }

    [Test]
    public void AddRule_ShouldCreateRuleFromModel()
    {
        RuleModel empty = new RuleTemplateService().BuildEmpty();
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel testBlock = sut.Blocks.First();

        sut.NewRule(empty, testBlock);
        RuleDetailsViewModel result = testBlock.Rules.First();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Properties.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(result.Properties.Show, Is.EqualTo(empty.Show));
        Assert.That(result.Properties.Priority, Is.EqualTo(empty.Priority));
    }

    [Test]
    public void ResetBlockTemplate_ShouldRaiseMultipleRulesDeleted()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel vm = sut.Blocks.First();
        sut.NewRule(vm);
        sut.NewRule(vm);
        sut.NewRule(vm);
        sut.NewRule(vm);
        EventListener<MultipleRulesDeleted, MultipleRuleDeletedDetails> listener = new();
        sut.ResetBlockTemplate(vm, "Empty");

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload.Block, Is.EqualTo(vm));
    }

    [Test]
    public void ShouldDeleteRule_WhenRuleDeletedRequestRaised()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel vm = sut.Blocks.First();
        sut.NewRule(vm);
        BlockDetailsViewModel testBlock = sut.Blocks.First();
        RuleDetailsViewModel testRule = testBlock.Rules.First();

        WeakReferenceMessenger.Default.Send(new DeleteRuleRequest(testRule));

        Assert.That(testBlock.Rules, Has.Count.EqualTo(0));
    }

    [Test]
    public void DeleteRule_ShouldRaiseRuleDeletedEvent()
    {
        FilterViewModel sut = Build();
        sut.NewBlock();
        BlockDetailsViewModel vm = sut.Blocks.First();
        sut.NewRule(vm);
        BlockDetailsViewModel testBlock = sut.Blocks.First();
        RuleDetailsViewModel testRule = testBlock.Rules.First();
        EventListener<RuleDeleteEvent, RuleDetailsViewModel> listener = new();

        sut.DeleteRule(testRule);

        Assert.That(listener.Received, Is.True);
        Assert.That(listener.Playload, Is.EqualTo(testRule));
    }

    [Test]
    public void ShouldSortRules_WhenSortRulesRequestRaised()
    {
        WeakReferenceMessenger messenger = new();
        FilterViewModel sut = new FilterViewModel(messenger);
        sut.NewBlock();
        BlockDetailsViewModel testBlock = sut.Blocks.First();
        sut.NewRule(testBlock);
        sut.NewRule(testBlock);
        sut.NewRule(testBlock);
        RuleDetailsViewModel rule = testBlock.Rules.First();
        testBlock.Rules[1].Properties.Priority = 1000;
        testBlock.Rules[2].Properties.Priority = 10000;
        rule.Properties.Priority = 100;

        messenger.Send(new SortRulesRequest(rule));    

        Assert.That(testBlock.Rules, Has.Count.EqualTo(3));
        Assert.That(testBlock.Rules.IndexOf(rule), Is.EqualTo(2));
    }

    public static bool ModelMatchViewModel(FilterModel model, FilterViewModel vm)
    {
        if (!model.Name.Equals(vm.Name))
        {
            return false;
        }
        if (model.Blocks.Count != vm.Blocks.Count)
        {
            return false;
        }
        for (int i = 0; i < model.Blocks.Count; i++)
        {
            BlockModel blockModel = model.Blocks[i];
            BlockDetailsViewModel blockVm = vm.Blocks[i];
            if (!blockModel.Title.Equals(blockVm.Title))
            {
                return false;
            }
            if (blockModel.Enabled != blockVm.Enabled)
            {
                return false;
            }
            if (blockModel.Priority != blockVm.Priority)
            {
                return false;
            }
        }
        return true;
    }
}
