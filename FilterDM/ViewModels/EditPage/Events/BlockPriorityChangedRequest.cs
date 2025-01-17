using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockPriorityChangedRequest : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockPriorityChangedRequest(BlockDetailsViewModel value) : base(value)
    {
    }
}



