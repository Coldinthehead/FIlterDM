using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FilterDM.ViewModels.EditPage.Events;
public class CreateBlockRequest : ValueChangedMessage<object>
{
    public CreateBlockRequest(object value) : base(value)
    {
    }
}
