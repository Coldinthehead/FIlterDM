using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels;
public partial class FilterViewModel : ViewModelBase
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
        blockVm.Title = "Block";
        Blocks.Add(blockVm);
    }
}
