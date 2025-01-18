
using FilterDM.ViewModels;

namespace FilterDM.Tests.ViewModel.Tests;
public class FilterViewModelTests
{
    [Test]
    public void NewBlock_ShouldAddEmptyBlockToBlocks()
    {
        FilterViewModel sut = new FilterViewModel();

        sut.NewBlock();

        Assert.That(sut.Blocks, Has.Count.EqualTo(1));
    }

}
