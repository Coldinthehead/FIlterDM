using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FilterDM.ViewModels.EditPage;



public partial class StructureTreeViewModel : ObservableRecipient
{
    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _blocks;

    [ObservableProperty]
    private ObservableRecipient _selectedNode;

    partial void OnSelectedNodeChanged(ObservableRecipient? oldValue, ObservableRecipient newValue)
    {
        if (newValue is  BlockDetailsViewModel block)
        {
            Messenger.Send(new BlockSelectedRequestEvent(block));
        }
        else if (newValue is RuleDetailsViewModel rule)
        {
            Messenger.Send(new RuleSelectedRequestEvent(rule));
        }
    }

    [RelayCommand]
    private void NewBlock()
    {
        var model = App.Current.Services.GetService<BlockTemplateService>().GetEmpty();
        var templateService = App.Current.Services.GetService<BlockTemplateService>();
        var itemTypeService = App.Current.Services.GetService<ItemTypeService>();
        model.Title = "block";
        var b = new BlockDetailsViewModel(Blocks, templateService.GetObservableNames(), new TypeScopeManager(itemTypeService));
        b.SetModel(model);
        Blocks.Add(b);
        Messenger.Send(new BlockCreatedRequestEvent(b));
    }

    private FilterModel _model;

    public void RemoveBlock(BlockDetailsViewModel block)
    {
        if (Blocks.Remove(block))
        {
            if (SelectedNode == block)
            {
                SelectedNode = null;
            }
        }
        
    }

    public StructureTreeViewModel(FilterModel model)
    {
        _model = model;
    }

    public void BindBlocks()
    {
        var templateService = App.Current.Services.GetService<BlockTemplateService>();
        var itemTypeService = App.Current.Services.GetService<ItemTypeService>();
        ObservableCollection<BlockDetailsViewModel> next = new();
        foreach (var item in _model.Blocks)
        {
            var vm = new BlockDetailsViewModel(next, templateService.GetObservableNames(), new TypeScopeManager(itemTypeService));
            vm.SetModel(item);
            next.Add(vm);
        }

        Blocks = next;
    }


    internal void SortBlocks()
    {
        List<BlockDetailsViewModel> sorted = [.. Blocks.ToList().OrderBy(x => x.CalculatedPriority)];
        Blocks = new ObservableCollection<BlockDetailsViewModel>(sorted);
    }
}
