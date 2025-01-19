using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class RuleCreatedListener : ObservableRecipient, IRecipient<RuleCreatedInFilter>
{
    public bool Received = false;
    public RuleDetailsViewModel Rule;

    public RuleCreatedListener()
    {
        Messenger.Register(this);
    }
    public void Receive(RuleCreatedInFilter message)
    {
        Received = true;
        Rule = message.Value;
    }
}
