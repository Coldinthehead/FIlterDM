using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class RuleCreateRequestEvent : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleCreateRequestEvent(RuleDetailsViewModel value) : base(value)
    {
    }
}

