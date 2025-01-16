using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;

public partial class BlockEditorViewModel : EditorBaseViewModel
    , IRecipient<RuleCloseRequestEvent>
{
    [ObservableProperty]
    private BlockDetailsViewModel _block;

    [ObservableProperty]
    private RuleDetailsViewModel _selectedRule;
    partial void OnSelectedRuleChanged(RuleDetailsViewModel? oldValue, RuleDetailsViewModel newValue)
    {
        if (newValue != null)
        {
            Messenger.Send(new RuleSelectedRequestEvent(newValue));
        }
    }

    [ObservableProperty]
    private ObservableCollection<string> _templates;

    [ObservableProperty]
    private string _selectedTempalte;


    [RelayCommand]
    private void DeleteCurrent()
    {
        Messenger.Send(new BlockDeleteRequestEvent(Block));
        Messenger.Send(new FilterEditedRequestEvent(this));
        Block = null;
    }


    [RelayCommand]
    public void ApplyChanges()
    {
        Messenger.Send(new BlockPriorityChangedRequest(this));
        Title = this.Block.Title;
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [RelayCommand]
    private async void Reset()
    {
        if (SelectedTempalte != null)
        {
            if (Block.Rules.Count > 0)
            {
                var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to override {Block.Rules.Count} rules?");
                if (!confirm)
                {
                    return;
                }
            }


            var service = App.Current.Services.GetService<BlockTemplateService>();
            BlockModel? nextTeplate = service.GetTemplate(SelectedTempalte);
            if (nextTeplate != null)
            {
                nextTeplate.Title = this.Block.Title;
                Block.SetModel(nextTeplate);

            }
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
        Messenger.Send(new BlockPriorityChangedRequest(this));
    }

    public BlockEditorViewModel(BlockDetailsViewModel block) : base()
    {
        if (_templates == null)
        {
            var service = App.Current.Services.GetService<BlockTemplateService>();
            Templates = new ObservableCollection<string>(service.GetTempalteNames());
        }
        Block = block;
        Content = this;
        Title = block.Title;
        SelectedTempalte = "Empty";
        Messenger.Register<RuleCloseRequestEvent>(this);
    }

    public override bool IsPartOf(BlockDetailsViewModel vm)
    {
        return Block == vm;
    }

    public override bool Equals(object? obj)
    {
        if (obj is  BlockEditorViewModel other)
        {
            return Block == other.Block;
        }
        return false;
    }

    public void Receive(RuleCloseRequestEvent message)
    {
        if (SelectedRule == message.Value.Rule)
        {
            SelectedRule = null;
        }
    }
}
