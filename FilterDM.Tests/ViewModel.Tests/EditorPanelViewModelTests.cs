using FilterDM.ViewModels;
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

        Assert.That(sut.Items, Is.Empty);
    }
    [Test]
    public void AddBlock_ShouldCreateBlockEditor_WhenEditorNotExists()
    {
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = new BlockDetailsViewModel(new(), new(), new(new()));

        sut.AddBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.First().IsPartOf(testBlock), Is.True);
    }

    [Test]
    public void AddBlock_ShouldNotCreateBlockEdtiro_WhenEditorExists()
    {
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = new BlockDetailsViewModel(new(), new(), new(new()));
        sut.AddBlock(testBlock);

        sut.AddBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
    }

    [Test]
    public void ShouldOpenEditor_WhenBlockCreated()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel fitlerVm = new(new(), new());

        fitlerVm.NewBlock();
        BlockDetailsViewModel testBlock = fitlerVm.Blocks.First();

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldCloseEditor_WhenBlockDeleted()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel fitlerVm = new(new(), new());
        fitlerVm.NewBlock();
        fitlerVm.NewBlock();

        BlockDetailsViewModel testBlock = fitlerVm.Blocks.First();
        fitlerVm.DeleteBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x=>x.IsPartOf(testBlock)).First(), Is.False);
    }
}
