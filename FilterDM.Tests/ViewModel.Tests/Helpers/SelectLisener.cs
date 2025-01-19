using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class SelectLisener : ObservableRecipient
    , IRecipient<BlockSelectedInTree>
{
    public bool Recieved = false;
    public BlockDetailsViewModel Selection;

    public SelectLisener()
    {
        Messenger.Register(this);
    }

    public void Receive(BlockSelectedInTree message)
    {
        Recieved = true;
        Selection = message.Value;
    }
}
