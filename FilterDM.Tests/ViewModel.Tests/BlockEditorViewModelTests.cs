using FilterDM.Repositories;
using FilterDM.Tests.Helpers;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Tests.ViewModel.Tests;
public class BlockEditorViewModelTests
{
    [Test]
    public void CloseMeCommand_ShouldRaiseEvent()
    {
        BlockEditorViewModel editor = new BlockEditorViewModel(HelperFactory.GetBlock());
        CloseEditorListener listener = new();
        editor.CloseMeCommand.Execute(null);

        Assert.That(listener.Recieve, Is.True);
        Assert.That(listener.Editor, Is.EqualTo(editor));
    }

    [Test]
    public void ApplyCommand_ShouldRaiseEvent()
    {
        BlockEditorViewModel editor = new BlockEditorViewModel(HelperFactory.GetBlock());
        SortBlocksListener listener = new();

        editor.ApplyChangesCommand.Execute(null);

        Assert.That(listener.Received, Is.True);
    }
}
