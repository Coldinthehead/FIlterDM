using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels;
public partial class FilterViewModel : ObservableRecipient
    , IRecipient<CreateBlockRequest>
    , IRecipient<DeleteBlockRequest>
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    public ObservableCollection<BlockDetailsViewModel> _blocks;

    private ObservableCollection<string> _templateNames;

    private readonly ItemTypeService _typeService;
    private readonly BlockTemplateService _blockTempalteSerivice;
    private readonly RuleTemplateService _ruleTemplateService;

    public FilterViewModel(ItemTypeService typeService, BlockTemplateService blockTempalteSerivice, RuleTemplateService ruleTemplateService)
    {
        _typeService = typeService;
        _blockTempalteSerivice = blockTempalteSerivice;
        _ruleTemplateService = ruleTemplateService;
        _blocks = new();
        _templateNames = new();
        Messenger.Register<CreateBlockRequest>(this);
        Messenger.Register<DeleteBlockRequest>(this);   
    }
    public void NewBlock()
    {
        BlockDetailsViewModel blockVm = new(Blocks, _templateNames, new TypeScopeManager(_typeService));
        BlockModel template = _blockTempalteSerivice.GetEmpty();
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

    public void NewRule(BlockDetailsViewModel parent)
    {
        RuleDetailsViewModel ruleVm = new RuleDetailsViewModel(Blocks, parent);

        parent.AddRule(ruleVm);
    }

    public void SetModel(FilterModel model)
    {
        Name = model.Name;
        ObservableCollection<BlockDetailsViewModel> next = new();
        foreach (BlockModel blockModel in model.Blocks)
        {
            BlockDetailsViewModel blockVm = new BlockDetailsViewModel(Blocks, _templateNames, new TypeScopeManager(_typeService));
            blockVm.SetModel(blockModel);
            blockVm.Title = blockModel.Title;
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
    #endregion
}
