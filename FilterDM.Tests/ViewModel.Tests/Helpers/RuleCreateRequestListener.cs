using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class RuleCreateRequestListener : ObservableRecipient, IRecipient<CreateRuleRequest>
{
    public bool Received = false;
    public BlockDetailsViewModel Block;

    public RuleCreateRequestListener()
    {
        Messenger.Register(this);
    }
    public void Receive(CreateRuleRequest message)
    {
        Received = true;
        Block = message.Value;
    }
}
