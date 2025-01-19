
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class SortBlocksListener : ObservableRecipient, IRecipient<SortBlocksRequest>
{
    public bool Received = false;

    public SortBlocksListener()
    {
        Messenger.Register(this);
    }
    public void Receive(SortBlocksRequest message)
    {
        Received = true;
    }
}
