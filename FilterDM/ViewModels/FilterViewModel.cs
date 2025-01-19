using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
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
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    public ObservableCollection<BlockDetailsViewModel> _blocks;

    private ObservableCollection<string> _templateNames;

    private readonly ItemTypeService _typeService;
    private readonly RuleTemplateService _ruleTemplateService;
    private readonly BlockTemplateService _blockTemplateService;

    public FilterViewModel(ItemTypeService typeService, BlockTemplateService blockTempalteService, RuleTemplateService ruleTemplateService)
    {
        _typeService = typeService;
        _ruleTemplateService = ruleTemplateService;
        _blockTemplateService = blockTempalteService;
        _blocks = new();
        _templateNames = new();
        RegisterEvents();
    }
    public FilterViewModel(IMessenger messeneger) : base(messeneger)
    {
        _typeService = new();
        _ruleTemplateService = new();
        _blocks = new();
        _templateNames = new();
        _blockTemplateService = new(new BlockTemplateRepository());
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        Messenger.Register<CreateBlockRequest>(this);
        Messenger.Register<DeleteBlockRequest>(this);
        Messenger.Register<SortBlocksRequest>(this);
        Messenger.Register<CreateRuleRequest>(this);
        Messenger.Register<ResetTemplateRequest>(this);
    }

    public void NewBlock()
    {
        BlockDetailsViewModel blockVm = new(_templateNames, new TypeScopeManager(_typeService));
        BlockModel template = _blockTemplateService.GetEmpty();
        blockVm.SetModel(template);
        blockVm.Title = GetGenericBlockTitle();
        Blocks.Add(blockVm);
        Messenger.Send(new BlockInFilterCreated(blockVm));
    }

    public void DeleteBlock(BlockDetailsViewModel vm)
    {
        if (Blocks.Remove(vm))
        {
            Messenger.Send(new BlockDeletedInFilter(vm));
        }
    }

    public void ResetBlockTemplate(BlockDetailsViewModel block, string tempalteName)
    {
        if (_blockTemplateService.TryGetTemplate(tempalteName, out BlockModel template))
        {
            template.Title = block.Title;
            block.SetModel(template);
            List<RuleDetailsViewModel> currentRules = block.Rules.ToList();
            block.Rules.Clear();
            Messenger.Send(new MultipleRulesDeleted(new MultipleRuleDeletedDetails(block, currentRules)));
            foreach (RuleModel model in template.Rules)
            {
                NewRule(model, block);
            }
        }
    }

    public void SortBlocks()
    {
        List<BlockDetailsViewModel> next = Blocks.Select(x => x).OrderBy(x => x.CalculatedPriority).ToList();
        Blocks = new(next);
        Messenger.Send(new BlockCollectionInFilterChanged(Blocks));
    }

    public void NewRule(BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = new(Blocks, parent, _ruleTemplateService.GetObservableNames());
        ruleVm.SetModel(_ruleTemplateService.BuildEmpty());

        parent.AddRule(ruleVm);

        Messenger.Send(new RuleCreatedInFilter(ruleVm));
    }

    public void NewRule(RuleModel model, BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = new(Blocks, parent, _ruleTemplateService.GetObservableNames());
        ruleVm.SetModel(model);
        parent.AddRule(ruleVm);
        Messenger.Send(new RuleCreatedInFilter(ruleVm));
    }

    public void SetModel(FilterModel model)
    {
        Name = model.Name;
        ObservableCollection<BlockDetailsViewModel> next = new();
        foreach (BlockModel blockModel in model.Blocks)
        {
            BlockDetailsViewModel blockVm = new BlockDetailsViewModel(_templateNames, new TypeScopeManager(_typeService));
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
            ResetBlockTemplate(message.Value.Block, message.Value.TempalteName);
        }
    }
    #endregion
}
