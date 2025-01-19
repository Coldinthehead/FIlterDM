using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class RuleSelectListener : ObservableRecipient, IRecipient<RuleSelectedInTree>
{
    public bool Recieved = false;
    public RuleDetailsViewModel Selection;

    public RuleSelectListener()
    {
        Messenger.Register(this);
    }

    public void Receive(RuleSelectedInTree message)
    {
        Recieved = true;
        Selection = message.Value;
    }
}
