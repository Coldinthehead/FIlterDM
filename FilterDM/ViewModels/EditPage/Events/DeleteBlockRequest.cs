using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class DeleteBlockRequest : ValueChangedMessage<BlockDetailsViewModel>
{
    public DeleteBlockRequest(BlockDetailsViewModel value) : base(value)
    {
    }
}

