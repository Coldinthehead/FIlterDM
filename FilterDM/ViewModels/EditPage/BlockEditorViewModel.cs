using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage.Events;

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
    }

    [RelayCommand]
    private void DeleteCurrent()
    {
        Messenger.Send(new DeleteBlockRequest(Block));
        Messenger.Send(new FilterEditedRequestEvent(this));
        Block = null;
    }


    [RelayCommand]
    public void ApplyChanges()
    {
        Messenger.Send(new BlockPriorityChangedRequest(Block));
        Title = this.Block.Title;
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

   

    public BlockEditorViewModel(BlockDetailsViewModel block) : base()
    {
        
        Block = block;
        Content = this;
        Title = block.Title;
   
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
