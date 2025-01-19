using CommunityToolkit.Mvvm.Messaging.Messages;


namespace FilterDM.ViewModels.EditPage.Events;

public class ResetTemplateRequest : ValueChangedMessage<TemplateChangeDetils>
{
    public ResetTemplateRequest(TemplateChangeDetils value) : base(value)
    {
    }
}

