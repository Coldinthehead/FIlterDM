
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;

public class CloseEditorListener: ObservableRecipient , IRecipient<EditorClosedEvent>
{
    public bool Recieve = false;
    public EditorBaseViewModel Editor;

    public CloseEditorListener()
    {
        Messenger.Register(this);
    }

    public void Receive(EditorClosedEvent message)
    {
        Recieve = true;
        Editor = message.Value;
    }
}
