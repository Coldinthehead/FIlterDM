using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class DeleteRuleRequest : ValueChangedMessage<RuleDetailsViewModel>
{
    public DeleteRuleRequest(RuleDetailsViewModel value) : base(value)
    {
    }
}

