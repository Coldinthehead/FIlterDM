using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class FilterViewModelTests
{
    [Test]
    public void NewBlock_ShouldCreateEmptyBlock()
    {
        FilterViewModel sut = new FilterViewModel(new(), new(), new());

        sut.NewBlock();

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }


    [Test]
    public void NewBlock_ShouldApplyEmptyTemplateToNewBlock()
    {
        BlockTemplateRepository templateService = new();
        BlockModel empty = templateService.GetEmpty();
        FilterViewModel sut = new(new(), templateService, new());
        sut.NewBlock();
        BlockDetailsViewModel newBlock = sut.Blocks.First();

        Assert.That(newBlock.SelectedTempalte, Is.EqualTo(empty.TemplateName));
        Assert.That(newBlock.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(newBlock.Priority, Is.EqualTo(empty.Priority));
        Assert.That(newBlock.Rules, Has.Count.EqualTo(empty.Rules.Count));
    }

    [Test]
    public void NewBlock_ShouldSetBlockTitleToGenericName_WhenBlocksEmpty()
    {
        FilterViewModel sut = new(new(), new(), new());

        sut.NewBlock();

        Assert.That(sut.Blocks.First().Title, Is.EqualTo("Block"));
    }

    [Test]
    public void NewBlock_ShouldCreateBlockWithUniqueName()
    {
        FilterViewModel sut = new(new(), new(), new());
        sut.NewBlock();
        sut.NewBlock();
        BlockDetailsViewModel first = sut.Blocks.First();

        HashSet<string> titles = [.. sut.Blocks.Select(x => x.Title)];
        Assert.That(titles, Has.Count.EqualTo(sut.Blocks.Count));
    }

    [Test]
    public void NewBlock_ShouldRaiseBlockCreatedEvent()
    {
        FilterViewModel sut = new(new(), new(), new());
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
        FilterViewModel sut = new(new(), new(), new());

        WeakReferenceMessenger.Default.Send(new CreateBlockRequest(null));

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }

    [Test]
    public void SetModel_ShouldReplicateBlocks()
    {
        FilterViewModel sut = new(new(), new(), new());
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
        FilterViewModel sut = new(new(), new(), new());
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
        FilterViewModel sut = new(new(), new(), new());
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();

        WeakReferenceMessenger.Default.Send(new DeleteBlockRequest(block));

        Assert.That(sut.Blocks, Does.Not.Contain(block));
    }

    [Test]
    public void DeleteBlock_ShouldRaiseBlockDeletedEvent()
    {
        FilterViewModel sut = new(new(), new(), new());
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
        FilterViewModel sut = new(new(), new(), new());
        sut.NewBlock();
        BlockDetailsViewModel block = sut.Blocks.First();

        sut.NewRule(block);

        Assert.That(block.Rules, Has.Count.EqualTo(1));
    }

    [Test]
    public void NewRule_ShouldSetEmptyTemplate()
    {
        RuleTemplateService service = new();
        FilterViewModel sut = new(new(), new(), service);
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
        FilterViewModel sut = new(new(), new(), service);
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
        FilterViewModel sut = new(new(), new(), service);
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
        FilterViewModel sut = new(new(), new(), service);
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
