using CommunityToolkit.Mvvm.Messaging.Messages;
using FilterDM.Models;


namespace FilterDM.ViewModels.EditPage.Events;

public class BlockCloseRequestEvent : ValueChangedMessage<BlockEditorViewModel>
{
    public BlockCloseRequestEvent(BlockEditorViewModel value) : base(value)
    {
    }
}


public readonly struct BlockModelChangedDetails
{
    public readonly BlockModel Old;
    public readonly BlockModel New;

    public BlockModelChangedDetails(BlockModel old, BlockModel @new)
    {
        Old = old;
        New = @new;
    }
}
public class BlockModelChangedEvent : ValueChangedMessage<BlockModelChangedDetails>
{
    public BlockModelChangedEvent(BlockModelChangedDetails value) : base(value)
    {
    }
}


