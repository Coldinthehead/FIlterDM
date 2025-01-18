using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;

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
        FilterViewModel fitlerVm = new(new(), new(), new());

        fitlerVm.NewBlock();
        BlockDetailsViewModel testBlock = fitlerVm.Blocks.First();

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldCloseEditor_WhenBlockDeleted()
    {
        EditorPanelViewModel sut = new();
        FilterViewModel fitlerVm = new(new(), new(), new());
        fitlerVm.NewBlock();
        fitlerVm.NewBlock();

        BlockDetailsViewModel testBlock = fitlerVm.Blocks.First();
        fitlerVm.DeleteBlock(testBlock);

        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(sut.Items.Select(x=>x.IsPartOf(testBlock)).First(), Is.False);
    }

    [Test]
    public void ShouldOpenTab_WhenBlockSelectedInTree()
    {
        FilterViewModel fitlerVm = new(new(), new(), new());
        fitlerVm.NewBlock();
        fitlerVm.NewBlock();
        StructureTreeViewModel tree = new();
        tree.SetBlocks(fitlerVm.Blocks);
        EditorPanelViewModel sut = new();
        BlockDetailsViewModel testBlock = fitlerVm.Blocks.First();

        tree.Select(testBlock);

        Assert.That(sut.Items.Select(x => x.IsPartOf(testBlock)).First(), Is.True);
    }

    [Test]
    public void ShouldRaiseEvent_WhenTabClosed()
    {
        FilterViewModel fitlerVm = new(new(), new(), new());
        EditorPanelViewModel sut = new();
        fitlerVm.NewBlock();
        EditorBaseViewModel testEditor = sut.Items.First();
        EditorCloseListener listener = new();
        testEditor.CloseMeCommand.Execute(null);

        Assert.That(listener.Recieved, Is.True);
        Assert.That(listener.Editor.Block, Is.EqualTo(fitlerVm.Blocks.First()));
    }

    [Test]
    public void ShouldOpenEditor_WhenRuleSelectedEvent()
    {
        FilterViewModel fitlerVm = new(new(), new(), new());
        fitlerVm.NewBlock();
        BlockDetailsViewModel block = fitlerVm.Blocks.First();
        fitlerVm.NewRule(block);
        RuleDetailsViewModel testRule = block.Rules.First();
        EditorPanelViewModel sut = new();

        WeakReferenceMessenger.Default.Send(new RuleSelectedInTree(testRule));

        RuleEditorViewModel editor = sut.Items.First() as RuleEditorViewModel;
        Assert.That(sut.Items, Has.Count.EqualTo(1));
        Assert.That(editor, Is.Not.Null);
        Assert.That(editor.Rule, Is.EqualTo(testRule));
    }


    public class EditorCloseListener : ObservableRecipient, IRecipient<BlockEditorClosed>
    {
        public bool Recieved = false;
        public BlockEditorViewModel Editor;
        public EditorCloseListener()
        {
            Messenger.Register(this);
        }
        public void Receive(BlockEditorClosed message)
        {
            Recieved = true;
            Editor = message.Value;
        }
    }
}
