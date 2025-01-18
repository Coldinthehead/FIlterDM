
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockEditorViewModelTests
{
    [Test]
    public void CloseMeCommand_ShouldRaiseEvent()
    {
        BlockEditorViewModel editor = new BlockEditorViewModel(new BlockDetailsViewModel([], [], new(new())));
        CloseEditorListener listener = new();
        editor.CloseMeCommand.Execute(null);

        Assert.That(listener.Recieve, Is.True);
        Assert.That(listener.Editor, Is.EqualTo(editor));
    }

    [Test]
    public void ApplyCommand_ShouldRaiseEvent()
    {
        BlockEditorViewModel editor = new BlockEditorViewModel(new BlockDetailsViewModel([], [], new(new())));
        SortBlocksListener listener = new();

        editor.ApplyChangesCommand.Execute(null);

        Assert.That(listener.Received, Is.True);
    }

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
}
