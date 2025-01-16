
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilterDM.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.Pages;

partial class ProjectViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title;

    public Action<ProjectViewModel> DeleteMeAction { set; get; }

    [RelayCommand]
    private void DeleteMe()
    {
        DeleteMeAction?.Invoke(this);
    }
}

partial class ProjectPageViewModel : ViewModelBase
{
    [NotifyCanExecuteChangedFor(nameof(NewFilterCommand))]
    [ObservableProperty]
    private string _newFilterName;

    [ObservableProperty]
    private string _selectedFilterPreset;

    [ObservableProperty]
    private ObservableCollection<string> _availableFilterPresets;

    [ObservableProperty]
    private ObservableCollection<ProjectViewModel> _recentProjects;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadFilterCommand))]
    private ProjectViewModel _selectedProject;


    [RelayCommand(CanExecute = nameof(CanCreateFilter))]
    private void NewFilter()
    {
        CreateFilterAction?.Invoke(_newFilterName, _selectedFilterPreset);

    }
    [RelayCommand(CanExecute =nameof(CanLoadFilter))]
    private void LoadFilter()
    {
        ChoseFilterAction?.Invoke(SelectedProject.Title);
    }

    private bool CanCreateFilter()
    {
        return NewFilterName.Length > 2;
    }

    private bool CanLoadFilter()
    {
        return SelectedProject != null;
    }

    internal void OnEnter(List<string> projectsList)
    {
        List<ProjectViewModel> projects = [];
        foreach (var name in projectsList)
        {
            projects.Add(new ProjectViewModel()
            {
                Title = name,
                DeleteMeAction = DeleteProject,
            });
        }
        RecentProjects = new(projects);
        SelectedProject = null;
        NewFilterName = "";
        SelectedFilterPreset = AvailableFilterPresets[0];
    }

    public Action<string, string> CreateFilterAction { get; set; }

    public Action<string> ChoseFilterAction { get; set; }

    public ProjectPageViewModel()
    {
        AvailableFilterPresets = new ObservableCollection<string>(["Empty"]);
        SelectedFilterPreset = AvailableFilterPresets[0];
        NewFilterName = "";
    }

    private async void DeleteProject(ProjectViewModel project)
    {
        var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to delete {project.Title}?");
        if (confirm)
        {
            App.Current.Services.GetService<ProjectRepositoryService>().DeleteByName(project.Title);
            if (SelectedProject == project)
            {
                SelectedProject = null;
            }
            RecentProjects.Remove(project);
        }
    }

}
