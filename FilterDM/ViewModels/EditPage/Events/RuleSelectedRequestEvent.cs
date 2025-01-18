using CommunityToolkit.Mvvm.Messaging.Messages;
using FilterDM.ViewModels.EditPage.Decorators;


namespace FilterDM.ViewModels.EditPage.Events;



public class RuleModifierDeleteEvent : ValueChangedMessage<ModifierViewModelBase>
{
    public RuleModifierDeleteEvent(ModifierViewModelBase value) : base(value)
    {
    }
}

public class RuleTitleApplyEvent : ValueChangedMessage<RuleDetailsViewModel>
{
    public RuleTitleApplyEvent(RuleDetailsViewModel value) : base(value)
    {
    }
}

