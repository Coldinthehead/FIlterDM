using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockModelChangedEvent : ValueChangedMessage<BlockModelChangedDetails>
{
    public BlockModelChangedEvent(BlockModelChangedDetails value) : base(value)
    {
    }
}


