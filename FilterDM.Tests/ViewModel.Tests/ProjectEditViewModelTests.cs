using FilterDM.Models;
using FilterDM.ViewModels.Pages;

namespace FilterDM.Tests.ViewModel.Tests;
public class ProjectEditViewModelTests
{
    private FilterModel _model;
    [SetUp]
    public void SetUp()
    {
        _model = new FilterModel();
        _model.Name = "Test";
        _model.AddBlock(new BlockModel()
        {
            Title = "block0",
        }); 
        _model.AddBlock(new BlockModel()
        {
            Title = "block1",
        });
    }

    [Test]
    public void OnEnter_ShouldCreateFilterViewModel()
    {
        ProjectEditViewModel sut = new(new(), new());

        sut.OnEnter(_model);

        Assert.That(sut.Name, Is.EqualTo(_model.Name));
    }

    [Test]
    public void OnEnter_ShouldUpdateScturcureViewModel()
    {
        ProjectEditViewModel sut = new(new(), new());

        sut.OnEnter(_model);

        Assert.That(sut.FilterTree.Blocks, Has.Count.EqualTo(_model.Blocks.Count));
    }

    [Test]
    public void OnEnter_ShouldCloseAllEditors()
    {
        ProjectEditViewModel sut = new(new(), new());

        sut.OnEnter(_model);

        Assert.That(sut.EditorPanel.Items, Has.Count.EqualTo(0));
    }
}
