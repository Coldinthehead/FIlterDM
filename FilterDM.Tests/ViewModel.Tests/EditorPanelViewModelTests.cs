using FilterDM.ViewModels.EditPage;

namespace FilterDM.Tests.ViewModel.Tests;
public class EditorPanelViewModelTests
{
    [Test]
    public void Clear_ShouldDeleteAllEditors()
    {
        EditorPanelViewModel sut = new();
        sut.AddBlock(new BlockDetailsViewModel(new(), new(), new(new())));

        sut.Clear();

        Assert.That(sut.Items.Count, Is.EqualTo(0));
    }
}
