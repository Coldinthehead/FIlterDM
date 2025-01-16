using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;
public partial class BlockToolbarViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _blocks = new();

    [ObservableProperty]
    private BlockDetailsViewModel _selectedBlock;

    partial void OnSelectedBlockChanged(BlockDetailsViewModel? oldValue, BlockDetailsViewModel newValue)
    {
        if (oldValue != null)
        {
            oldValue.IsSelected = false;
        }
        if (newValue != null)
        {
            newValue.IsSelected = true;
            Messenger.Send(new BlockSelectedRequestEvent(newValue));
        }
    }


    [RelayCommand]
    private void NewBlock()
    {
        var model = App.Current.Services.GetService<BlockTemplateService>().GetEmpty();
        model.Title = _model.GetGenericBlockTitle();
        _model.AddBlock(model);
        var vm = new BlockDetailsViewModel(Blocks);
        vm.SetModel(model);
        Blocks.Add(vm);
    }


    public void RemoveBlock(BlockDetailsViewModel block)
    {
        Blocks.Remove(block);
    }

    public BlockToolbarViewModel()
    {
        _model = new();
    }

    public FilterModel Model => _model;
    private FilterModel _model;
    public BlockToolbarViewModel(FilterModel model)
    {
        _model = model;
    }
}
