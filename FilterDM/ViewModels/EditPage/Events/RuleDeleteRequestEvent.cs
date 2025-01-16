using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class RuleDeleteRequestEvent : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleDeleteRequestEvent(RuleDetailsViewModel value) : base(value)
    {
    }
}

