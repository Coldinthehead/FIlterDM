using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockPriorityChangedRequest : ValueChangedMessage<BlockEditorViewModel>
{
    public BlockPriorityChangedRequest(BlockEditorViewModel value) : base(value)
    {
    }
}



