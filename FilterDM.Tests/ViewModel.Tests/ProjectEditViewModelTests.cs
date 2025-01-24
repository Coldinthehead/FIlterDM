﻿using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.Pages;

namespace FilterDM.Tests.ViewModel.Tests;
public class ProjectEditViewModelTests
{
    private FilterModel _model;
    private ProjectEditViewModel _sut;
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
        _sut = new (
            new ItemTypeService(), new BlockTemplateService(new BlockTemplateRepository())
            , new RuleTemplateService(new RuleTemplateRepository())
            , new ProjectService(new ProjectRepository(null), new DialogService(null))
            , new FileSelectionService(null)
            , new FileService(new DialogService(null))
            , new FilterExportService()
            , new MinimapIconsService()
            , new SoundService());
    }

    [Test]
    public void OnEnter_ShouldCreateFilterViewModel()
    {

        _sut.OnEnter(_model);

        Assert.That(_sut.Name, Is.EqualTo(_model.Name));
    }

    [Test]
    public void OnEnter_ShouldUpdateScturcureViewModel()
    {

        _sut.OnEnter(_model);

        Assert.That(_sut.FilterTree.Blocks, Has.Count.EqualTo(_model.Blocks.Count));
    }

    [Test]
    public void OnEnter_ShouldCloseAllEditors()
    { 
        _sut.OnEnter(_model);

        Assert.That(_sut.EditorPanel.Items, Has.Count.EqualTo(0));
    }
}
