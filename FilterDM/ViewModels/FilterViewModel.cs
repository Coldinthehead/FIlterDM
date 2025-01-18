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
{
    [ObservableProperty]
    public ObservableCollection<BlockDetailsViewModel> _blocks;

    private ObservableCollection<string> _templateNames;

    private readonly ItemTypeService _typeService;
    private readonly BlockTemplateService _blockTempalteSerivice;

    public FilterViewModel(ItemTypeService typeService, BlockTemplateService blockTempalteSerivice)
    {
        _typeService = typeService;
        _blockTempalteSerivice = blockTempalteSerivice;
        _blocks = new();
        _templateNames = new();
    }
    public void NewBlock()
    {
        BlockDetailsViewModel blockVm = new(Blocks, _templateNames, new TypeScopeManager(_typeService));
        BlockModel template = _blockTempalteSerivice.GetEmpty();
        blockVm.SetModel(template);
        blockVm.Title = GetGenericBlockTitle();
        Blocks.Add(blockVm);
        Messenger.Send(new BlockCreatedRequestEvent(blockVm));
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
}
