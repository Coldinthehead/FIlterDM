using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FilterDM.ViewModels.EditPage.Events;

public class BlockInFilterCreated : ValueChangedMessage<BlockDetailsViewModel>
{
    public BlockInFilterCreated(BlockDetailsViewModel value) : base(value)
    {
    }
}
