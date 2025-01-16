using CommunityToolkit.Mvvm.Messaging.Messages;
using FilterDM.Models;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockEnabledChangedEvent : ValueChangedMessage<BlockModel>
{
    public BlockEnabledChangedEvent(BlockModel value) : base(value)
    {
    }
}
