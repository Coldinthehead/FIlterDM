using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.ViewModels.EditPage;

public partial class BlockEditorViewModel : EditorBaseViewModel
{
    [ObservableProperty]
    private BlockDetailsViewModel _block;

    [ObservableProperty]
    private RuleDetailsViewModel _selectedRule;
    partial void OnSelectedRuleChanged(RuleDetailsViewModel? oldValue, RuleDetailsViewModel newValue)
    {
        if (newValue != null)
        {
            SelectedRule = null;
            Messenger.Send(new RuleSelectedInTree(newValue));
        }
    }

    [RelayCommand]
    public void ApplyChanges()
    {
        Title = this.Block.Title;
        Messenger.Send(new SortBlocksRequest(Block));
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public BlockEditorViewModel(BlockDetailsViewModel block) : base()
    {
        
        Block = block;
        Content = this;
        Title = block.Title;
    }

    public override bool IsPartOf(BlockDetailsViewModel vm)
    {
        return Block == vm;
    }

    public override ObservableRecipient GetSelectedContext()
    {
        return Block;
    }

    public override void UpdateTitle()
    {
        Title = Block.Title;
    }

    public override bool Equals(object? obj)
    {
        if (obj is  BlockEditorViewModel other)
        {
            return Block == other.Block;
        }
        return false;
    }
}
