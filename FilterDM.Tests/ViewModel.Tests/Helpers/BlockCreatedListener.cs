using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class BlockCreatedListener : ObservableRecipient, IRecipient<BlockInFilterCreated>
{
    public bool Recieved = false;
    public BlockDetailsViewModel? EventModel;
    public BlockCreatedListener()
    {
        Messenger.Register(this);
    }

    public void Receive(BlockInFilterCreated message)
    {
        Recieved = true;
        EventModel = message.Value;
    }
}
