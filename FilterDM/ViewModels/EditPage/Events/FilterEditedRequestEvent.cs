using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class FilterEditedRequestEvent : ValueChangedMessage<object>
{
    public FilterEditedRequestEvent(object value) : base(value)
    {
    }
}

