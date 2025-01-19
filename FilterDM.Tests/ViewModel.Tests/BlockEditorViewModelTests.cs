using FilterDM.ViewModels.EditPage;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockEditorViewModelTests
{
    [Test]
    public void CloseMeCommand_ShouldRaiseEvent()
    {
        BlockEditorViewModel editor = new BlockEditorViewModel(new BlockDetailsViewModel ([], new(new())));
        CloseEditorListener listener = new();
        editor.CloseMeCommand.Execute(null);

        Assert.That(listener.Recieve, Is.True);
        Assert.That(listener.Editor, Is.EqualTo(editor));
    }

    [Test]
    public void ApplyCommand_ShouldRaiseEvent()
    {
        BlockEditorViewModel editor = new BlockEditorViewModel(new BlockDetailsViewModel([], new(new())));
        SortBlocksListener listener = new();

        editor.ApplyChangesCommand.Execute(null);

        Assert.That(listener.Received, Is.True);
    }
}
