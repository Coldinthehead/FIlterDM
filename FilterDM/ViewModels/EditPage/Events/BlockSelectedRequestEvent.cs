using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockSelectedRequestEvent : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockSelectedRequestEvent(BlockDetailsViewModel value) : base(value)
    {
    }

}
