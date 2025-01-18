
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Tests.ViewModel.Tests;
public class FilterViewModelTests
{
    [Test]
    public void NewBlock_ShouldCreateEmptyBlock()
    {
        FilterViewModel sut = new FilterViewModel(new ItemTypeService(), new BlockTemplateService());

        sut.NewBlock();

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }


    [Test]
    public void NewBlock_ShouldApplyEmptyTemplateToNewBlock()
    {
        BlockTemplateService templateService = new();
        BlockModel empty = templateService.GetEmpty();
        FilterViewModel sut = new(new ItemTypeService(), templateService);
        sut.NewBlock();
        BlockDetailsViewModel newBlock = sut.Blocks.First();

        Assert.That(newBlock.SelectedTempalte, Is.EqualTo(empty.TemplateName));
        Assert.That(newBlock.Enabled, Is.EqualTo(empty.Enabled));
        Assert.That(newBlock.Priority, Is.EqualTo(empty.Priority));
        Assert.That(newBlock.Rules, Has.Count.EqualTo(empty.Rules.Count));
    }

    [Test]
    public void NewBlock_ShouldSetBlockTitleToGenericName_WhenBlocksEmpty()
    {
        FilterViewModel sut = new(new(), new());

        sut.NewBlock();

        Assert.That(sut.Blocks.First().Title, Is.EqualTo("Block"));
    }

    [Test]
    public void NewBlock_ShouldCreateBlockWithUniqueName()
    {
        FilterViewModel sut = new(new ItemTypeService(), new BlockTemplateService());
        sut.NewBlock();
        sut.NewBlock();
        BlockDetailsViewModel first = sut.Blocks.First();

        BlockDetailsViewModel blockWithDuplicateTitle = sut.Blocks.Skip(1).First(x => x.Title.Equals(first.Title));
        Assert.That(blockWithDuplicateTitle, Is.Null);
    }

}
