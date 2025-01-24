using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Factories;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
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
    , IRecipient<ResetRuleTemplateRequest>
{
    [ObservableProperty]
    private string _name;

    public Guid Guid { get; set; }

    private readonly PalleteManager _palleteManager;
    private readonly RuleParentManager _parentManager;

    private readonly IBlockViewModelFactory _blockFactory;
    private readonly IRuleViewModelFactory _ruleFactory;

    public FilterViewModel(PalleteManager palleteManager
        , RuleParentManager parentManager
        , IBlockViewModelFactory blockFactory
        , IRuleViewModelFactory ruleFactory)
    {
        RegisterEvents();
        _palleteManager = palleteManager;
        _parentManager = parentManager;
        _blockFactory = blockFactory;
        _ruleFactory = ruleFactory;
    }
    public FilterViewModel(IMessenger messeneger) : base(messeneger)
    {
        _parentManager = new();
        _palleteManager = new();
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
        Messenger.Register<ResetRuleTemplateRequest>(this);
    }

    public ObservableCollection<BlockDetailsViewModel> GetBlocks()
    {
        return _parentManager.AllBlocks;
    }

    public void NewBlock()
    {
        BlockDetailsViewModel blockVm = _blockFactory.BuildBlockViewModel();
        blockVm.Title = GetGenericBlockTitle();
        _parentManager.AllBlocks.Add(blockVm);
        Messenger.Send(new BlockInFilterCreated(blockVm));
    }

    public void DeleteBlock(BlockDetailsViewModel vm)
    {
        if (_parentManager.RemoveBlock(vm))
        {
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
        List<BlockDetailsViewModel> next = _parentManager.AllBlocks.Select(x => x).OrderBy(x => x.CalculatedPriority).ToList();
        ObservableCollection<BlockDetailsViewModel> nextBlocks = new(next);
        _parentManager.SetBlocks(nextBlocks);
        Messenger.Send(new BlockCollectionInFilterChanged(_parentManager.AllBlocks));
    }

    public void SortRules(BlockDetailsViewModel block, RuleDetailsViewModel sender)
    {
        block.SortRules();
        Messenger.Send(new SelectRuleInTreeRequest(sender));
    }

    public void NewRule(BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = _ruleFactory.BuildRuleViewModel(parent, _parentManager, _palleteManager);
        parent.AddRule(ruleVm);
        Messenger.Send(new RuleCreatedInFilter(ruleVm));
    }

    public void NewRule(RuleModel model, BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = _ruleFactory.BuildRuleViewModel(parent, _parentManager, _palleteManager, model);
        parent.AddRule(ruleVm);
        Messenger.Send(new RuleCreatedInFilter(ruleVm));
    }

    public void DeleteRule(RuleDetailsViewModel rule)
    {
        if (_parentManager.DeleteRule(rule))
        {
            Messenger.Send(new RuleDeleteEvent(rule));
        }
    }

    public void ResetRuleTemplate(RuleDetailsViewModel rule, RuleModel template)
    {
        var currentTitle = rule.Properties.Title;
        rule.SetModel(template);
        rule.Properties.Title = currentTitle;
    }

    public void SetModel(FilterModel model)
    {
        Name = model.Name;
        Guid = model.ID;
        ObservableCollection<BlockDetailsViewModel> next = new();
        foreach (BlockModel blockModel in model.Blocks)
        {
            BlockDetailsViewModel blockVm = _blockFactory.BuildBlockViewModel();
            blockVm.SetModel(blockModel);
            next.Add(blockVm);
            foreach (RuleModel rule in blockModel.Rules)
            {
                NewRule(rule, blockVm);
            }
        }
        _parentManager.SetBlocks(next);
        Messenger.Send(new BlockCollectionInFilterChanged(_parentManager.AllBlocks));
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
        => _parentManager.AllBlocks.Select(x => x.Title).Any((t) => string.Equals(title, t));


    public FilterModel GetModel()
    {
        FilterModel result = new()
        {
            Name = Name,
            ID = Guid,
        };
        foreach (BlockDetailsViewModel block in _parentManager.AllBlocks)
        {
            BlockModel resultBlock = block.GetModel();
            foreach (RuleDetailsViewModel rule in block.Rules)
            {
                RuleModel resultRule = rule.GetModel();
                resultBlock.AddRule(resultRule);
            }
            result.AddBlock(resultBlock);
        }
        return result;
    }

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
        if (_parentManager.AllBlocks.Contains(message.Value))
        {
            NewRule(message.Value);
        }
    }

    public void Receive(ResetTemplateRequest message)
    {
        if (_parentManager.AllBlocks.Contains(message.Value.Block))
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
        foreach (var block in _parentManager.AllBlocks)
        {
            if (block.Rules.Contains(message.Value))
            {
                SortRules(block, message.Value);
            }
        }
    }
    public void Receive(ResetRuleTemplateRequest message) => ResetRuleTemplate(message.Value.Rule, message.Value.Template);
    #endregion
}
