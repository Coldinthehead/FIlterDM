using CommunityToolkit.Mvvm.ComponentModel;
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

    public FilterViewModel(ItemTypeService typeService)
    {
        _typeService = typeService;
        _blocks = new();
        _templateNames = new();
    }
    public void NewBlock()
    {
        BlockDetailsViewModel blockVm = new(Blocks, _templateNames, new TypeScopeManager(_typeService));
        Blocks.Add(blockVm);
    }
}
