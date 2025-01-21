using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FilterDM.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private object _currentPage;


    private ProjectEditViewModel _editorViewModel;
    private ProjectPageViewModel _projectsPageViewModel;

    public MainWindowViewModel()
    {
        
    }

    public void Initialize(IServiceProvider services)
    {
        var typeService = services.GetService<ItemTypeService>();
        var blockTeplateService = services.GetService<BlockTemplateService>();
        var ruleTemplateService = services.GetService<RuleTemplateService>();
        var projectService = services.GetService<IProjectService>();
        var fileSelectionService = services.GetService<FileSelectionService>();
        var fileService = services.GetService<FileService>();
        _editorViewModel = new(typeService, blockTeplateService, ruleTemplateService, projectService, fileSelectionService, fileService)
        {
            BackToMenuAction = EnterProjectsPage,
        };
        _projectsPageViewModel = new()
        {
            CreateFilterAction = NewFilterCreated,
            ChoseFilterAction = FilterSelected,

        };
        CurrentPage = _projectsPageViewModel;
    }

    public void EnterProjectsPage()
    {
        var projectsList = App.Current.Services.GetService<ProjectRepository>().GetProjectNames;
        _projectsPageViewModel.OnEnter(projectsList);
        CurrentPage = _projectsPageViewModel;
    }

    private void FilterSelected(string filterName)
    {
        var filter = App.Current.Services.GetService<ProjectRepository>().GetProjectModel(filterName);
        if (filter != null)
        {
            _editorViewModel.OnEnter(filter);
            CurrentPage = _editorViewModel;
        }
    }

    private void NewFilterCreated(string filterName, string filterPreset)
    {
        var model = new FilterModel()
        {
            Name = filterName,
            ID = Guid.NewGuid(),
            LastSaveDate = DateTime.Now
        };

        var filterRepos = App.Current.Services.GetService<ProjectRepository>();
        filterRepos.CreateFilter(model);
        _editorViewModel.OnEnter(model);
        CurrentPage = _editorViewModel;
    }
}
