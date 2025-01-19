using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels;
public partial class FilterViewModel : ObservableRecipient
    , IRecipient<CreateBlockRequest>
    , IRecipient<DeleteBlockRequest>
    , IRecipient<SortBlocksRequest>
    , IRecipient<CreateRuleRequest>
    , IRecipient<ResetTemplateRequest>
    , IRecipient<DeleteRuleRequest>
    , IRecipient<SortRulesRequest>
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    public ObservableCollection<BlockDetailsViewModel> _blocks;

    private readonly ItemTypeService _typeService;
    private readonly RuleTemplateService _ruleTemplateService;
    private readonly BlockTemplateManager _blockTemplates;

    private readonly RuleParentManager _parentManager;

    public FilterViewModel(ItemTypeService typeService, BlockTemplateService blockTemplateService, RuleTemplateService ruleTemplateService)
    {
        _typeService = typeService;
        _ruleTemplateService = ruleTemplateService;
        _blockTemplates = new BlockTemplateManager(blockTemplateService);
        Blocks = new();
        _parentManager = new(Blocks);
        RegisterEvents();
    }
    public FilterViewModel(IMessenger messeneger) : base(messeneger)
    {
        _typeService = new();
        _ruleTemplateService = new();
        Blocks = new();
        _blockTemplates = new(new(new BlockTemplateRepository()));
        _parentManager = new(Blocks);
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        Messenger.Register<CreateBlockRequest>(this);
        Messenger.Register<DeleteBlockRequest>(this);
        Messenger.Register<SortBlocksRequest>(this);
        Messenger.Register<CreateRuleRequest>(this);
        Messenger.Register<ResetTemplateRequest>(this);
        Messenger.Register<DeleteRuleRequest>(this);
        Messenger.Register<SortRulesRequest>(this);
    }

    public void NewBlock()
    {
        BlockDetailsViewModel blockVm = new(_blockTemplates, new TypeScopeManager(_typeService));
        BlockModel template = _blockTemplates.GetEmpty();
        blockVm.SetModel(template);
        blockVm.Title = GetGenericBlockTitle();
        Blocks.Add(blockVm);
        _parentManager.AllBlocks.Add(blockVm);
        Messenger.Send(new BlockInFilterCreated(blockVm));
    }

    public void DeleteBlock(BlockDetailsViewModel vm)
    {
        if (Blocks.Remove(vm))
        {
            _parentManager.AllBlocks.Remove(vm);
            Messenger.Send(new BlockDeletedInFilter(vm));
        }
    }

    public void ResetBlockTemplate(BlockDetailsViewModel block, BlockModel template)
    {
        var currentTitle = block.Title;
        block.SetModel(template);
        block.Title = currentTitle;
        List<RuleDetailsViewModel> currentRules = block.Rules.ToList();
        block.Rules.Clear();
        Messenger.Send(new MultipleRulesDeleted(new MultipleRuleDeletedDetails(block, currentRules)));
        foreach (RuleModel model in template.Rules)
        {
            NewRule(model, block);
        }
    }

    public void SortBlocks()
    {
        List<BlockDetailsViewModel> next = Blocks.Select(x => x).OrderBy(x => x.CalculatedPriority).ToList();
        Blocks = new(next);
        _parentManager.SetBlocks(Blocks);
        Messenger.Send(new BlockCollectionInFilterChanged(Blocks));
    }

    public void SortRules(BlockDetailsViewModel block, RuleDetailsViewModel sender)
    {
        block.SortRules();
        Messenger.Send(new SelectRuleInTreeRequest(sender));
    }

    public void NewRule(BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = new(_parentManager, parent.ScopeManager, _ruleTemplateService.GetObservableNames());
        ruleVm.SetModel(_ruleTemplateService.BuildEmpty());
        parent.AddRule(ruleVm);
        Messenger.Send(new RuleCreatedInFilter(ruleVm));
    }

    public void NewRule(RuleModel model, BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = new(_parentManager, parent.ScopeManager, _ruleTemplateService.GetObservableNames());
        ruleVm.SetModel(model);
        parent.AddRule(ruleVm);
        Messenger.Send(new RuleCreatedInFilter(ruleVm));
    }

    public void DeleteRule(RuleDetailsViewModel rule)
    {
        foreach (BlockDetailsViewModel block in Blocks)
        {
            if (block.Rules.Contains(rule))
            {
                block.DeleteRule(rule);
                Messenger.Send(new RuleDeleteEvent(rule));
                break;
            }
        }
    }

    public void SetModel(FilterModel model)
    {
        Name = model.Name;
        ObservableCollection<BlockDetailsViewModel> next = new();
        foreach (BlockModel blockModel in model.Blocks)
        {
            BlockDetailsViewModel blockVm = new BlockDetailsViewModel(_blockTemplates, new TypeScopeManager(_typeService));
            blockVm.SetModel(blockModel);
            next.Add(blockVm);
        }
        Blocks = next;
        Messenger.Send(new BlockCollectionInFilterChanged(Blocks));
    }


    public string GetGenericBlockTitle()
    {
        int i = 0;
        string title = "Block";

        while (BlockTitleTaken(title))
        {
            title = $"Block({i++})";
        }
        return title;
    }

    private bool BlockTitleTaken(string title)
        => Blocks.Select(x => x.Title).Any((t) => string.Equals(title, t));


    internal FilterModel GetModel() => throw new NotImplementedException();

    #region event hadlers
    public void Receive(CreateBlockRequest message)
    {
        NewBlock();
    }

    public void Receive(DeleteBlockRequest message)
    {
        DeleteBlock(message.Value);
    }

    public void Receive(SortBlocksRequest message)
    {
        SortBlocks();
    }

    public void Receive(CreateRuleRequest message)
    {
        if (Blocks.Contains(message.Value))
        {
            NewRule(message.Value);
        }
    }

    public void Receive(ResetTemplateRequest message)
    {
        if (Blocks.Contains(message.Value.Block))
        {
            ResetBlockTemplate(message.Value.Block, message.Value.Template);
        }
    }

    public void Receive(DeleteRuleRequest message)
    {
        DeleteRule(message.Value);
    }

    public void Receive(SortRulesRequest message)
    {
        foreach (var block in Blocks)
        {
            if (block.Rules.Contains(message.Value))
            {
                SortRules(block, message.Value);
            }
        }
    }
    #endregion
}
