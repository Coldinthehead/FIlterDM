using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using System.Collections.ObjectModel;

namespace FilterDM.Tests.ViewModel.Tests;

public class BlockCollectionChangedListener : ObservableRecipient, IRecipient<BlockCollectionInFilterChanged>
{
    public ObservableCollection<BlockDetailsViewModel> Blocks;
    public bool Recieved = false;

    public BlockCollectionChangedListener()
    {
        Messenger.Register(this);
    }
    public void Receive(BlockCollectionInFilterChanged message)
    {
        Recieved = true;
        Blocks = message.Value;
    }
}

