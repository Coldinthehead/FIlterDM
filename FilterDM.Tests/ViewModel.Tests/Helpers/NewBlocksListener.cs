using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using System.Collections.ObjectModel;

namespace FilterDM.Tests.ViewModel.Tests;

public class NewBlocksListener : ObservableRecipient, IRecipient<BlockCollectionInFilterChanged>
{
    public bool Received = false;
    public ObservableCollection<BlockDetailsViewModel> Blocks;

    public NewBlocksListener()
    {
        Messenger.Register(this);
    }
    public void Receive(BlockCollectionInFilterChanged message)
    {
        Received = true;
        Blocks = message.Value;
    }
}
