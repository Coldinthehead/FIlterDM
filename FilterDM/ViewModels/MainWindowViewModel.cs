using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace FilterDM.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private object _currentPage;


    private ProjectEditViewModel _editorViewModel;
    private ProjectPageViewModel _projectsPageViewModel;

    public MainWindowViewModel()
    {
        _editorViewModel = new()
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
        var projectsList = App.Current.Services.GetService<ProjectRepositoryService>().GetProjectNames;
        _projectsPageViewModel.OnEnter(projectsList);
        CurrentPage = _projectsPageViewModel;
    }

    private void FilterSelected(string filterName)
    {
        var filter = App.Current.Services.GetService<ProjectRepositoryService>().GetProjectModel(filterName);
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
        };

        var filterRepos = App.Current.Services.GetService<ProjectRepositoryService>();
        filterRepos.CreateFilter(model);
        _editorViewModel.OnEnter(model);
        CurrentPage = _editorViewModel;
    }
}
