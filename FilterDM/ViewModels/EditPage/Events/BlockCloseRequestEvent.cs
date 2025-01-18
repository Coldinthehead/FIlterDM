using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockCloseRequestEvent : ValueChangedMessage<BlockEditorViewModel>
{
    public BlockCloseRequestEvent(BlockEditorViewModel value) : base(value)
    {
    }
}


