using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class RuleCloseRequestEvent : ValueChangedMessage<RuleEditorViewModel>
{
    public RuleCloseRequestEvent(RuleEditorViewModel value) : base(value)
    {
    }
}

