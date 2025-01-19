using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class BlockDeletedListener : ObservableRecipient, IRecipient<BlockDeletedInFilter>
{
    public bool Recieved = false;
    public BlockDetailsViewModel Block;

    public BlockDeletedListener()
    {
        Messenger.Register(this);
    }
    public void Receive(BlockDeletedInFilter message)
    {
        Recieved = true;
        Block = message.Value;  
    }
}
