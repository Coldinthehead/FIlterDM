using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockDeleteRequestEvent : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockDeleteRequestEvent(BlockDetailsViewModel value) : base(value)
    {
    }
}

