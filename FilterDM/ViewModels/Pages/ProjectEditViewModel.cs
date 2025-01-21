using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Repositories;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.ViewModels.Pages;

public partial class ProjectEditViewModel : ObservableRecipient, IRecipient<FilterEditedRequestEvent>
{

    [ObservableProperty]
    private StructureTreeViewModel _filterTree;
    [ObservableProperty]
    private EditorPanelViewModel _editorPanel;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private bool _changes = false;

    public Action BackToMenuAction { get; set; }

    private FilterViewModel _currentFilterVm;
    [RelayCommand]
    private async Task NewFilter()
    {
        if (Changes)
        {
            var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"You have unsaved changes that will be lost.Continue to Project menu?");
            if (confirm)
            {
                BackToMenuAction?.Invoke();
            }
        }
        else
        {
            BackToMenuAction.Invoke();
        }
    }

    [RelayCommand]
    private async Task Export()
    {
        FilterModel model = _currentFilterVm.GetModel();
        try
        {
            var filesService = App.Current?.Services?.GetService<FileSelectionService>();
            var file = await filesService.ExportFilterFile(model.Name);
            if (file != null)
            {
               /* App.Current.Services.GetService<SaveFilterService>().SaveModel(model, FilterTree.AllBlocks);*/
                var str = App.Current.Services.GetService<CoreFilterService>().Build(model);
                var path = file.Path.LocalPath;
                if (!Path.HasExtension(path) || !Path.GetExtension(path).Equals("filter"))
                {
                    path = Path.ChangeExtension(path, "filter");
                }
                using var fs = File.Create(path);
                using var sr = new StreamWriter(fs);
                sr.Write(str);
                await App.Current.Services.GetService<DialogService>().ShowOkDialog("Filter Exported!");
            }
        }
        catch (Exception ex)
        {

        }

    }
    [RelayCommand]
    private async Task SaveAs()
    {
        var file = await _fileSelectionService.SaveProjectFile(Name);
        if (file != null)
        {
            var result = await _fileService.Save(file, _currentFilterVm.GetModel());
        }

    }

    [RelayCommand]
    private async Task SaveCurrent()
    {
        bool result =  await _projectService.Save(_currentFilterVm.GetModel());
        Changes = !result;
    }

    [RelayCommand]
    private async Task Load()
    {
        if (Changes)
        {
            var confirm = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"You have unsaved changes that will be lost.Continue?");
            if (!confirm)
            {
                return;
            }
        }

        try
        {
            IStorageFile? file = await _fileSelectionService.OpenProjectFile();
            if (file == null)
            {
                return;
            }
            FilterModel model = await _fileService.LoadProject(file);
            OnEnter(model);
        }
        catch (Exception ex)
        {

        }

    }

    [RelayCommand]
    private async Task Parse()
    {
        try
        {
            var filesService = App.Current?.Services?.GetService<FileSelectionService>();
            if (filesService is null)
                throw new NullReferenceException("Missing File Service instance.");

            var file = await filesService.OpenProjectFile();
            if (file is null)
                return;


            string input = File.ReadAllText(file.Path.LocalPath);
            var result = App.Current.Services.GetService<FilterParserService>().Parse(input);
            if (result.ErorrMessage != "")
            {
                await App.Current.Services.GetService<DialogService>().ShowOkDialog($"Import Error: {result.ErorrMessage}");
            }
            if (result.ParseResult.Result != null)
            {
                var model = result.Model;
                model.Name = Path.GetFileNameWithoutExtension(file.Path.LocalPath);
                OnEnter(model);
                await App.Current.Services.GetService<DialogService>().ShowOkDialog($"Filter imported with {result.ParseResult.Result.Count} Rules");
            }
            else
            {
                await App.Current.Services.GetService<DialogService>().ShowOkDialog("Unable to recognize filter file.");
            }
        }
        catch (Exception e)
        {
        }
    }

    public void OnEnter(FilterModel model)
    {
        Name = model.Name;
        _currentFilterVm = new(_typeService, _blockTemplateService, _ruleTemplateService);
        _currentFilterVm.SetModel(model);

        FilterTree.SetBlocks(_currentFilterVm.GetBlocks());
        EditorPanel.Clear();
        Changes = false;
    }

    private readonly ItemTypeService _typeService;
    private readonly BlockTemplateService _blockTemplateService;
    private readonly RuleTemplateService _ruleTemplateService;
    private readonly IProjectService _projectService;
    private readonly FileSelectionService _fileSelectionService;
    private readonly FileService _fileService;

    public ProjectEditViewModel(ItemTypeService typeService
        , BlockTemplateService blockTempalteService
        , RuleTemplateService ruleTempalateService
        , IProjectService projectService
        , FileSelectionService fileSelectionService
        , FileService fileService)
    {
        _typeService = typeService;
        _blockTemplateService = blockTempalteService;
        FilterTree = new();
        EditorPanel = new();
        Messenger.Register<FilterEditedRequestEvent>(this);
        _ruleTemplateService = ruleTempalateService;
        _projectService = projectService;
        _fileSelectionService = fileSelectionService;
        _fileService = fileService;
    }

    public void Receive(FilterEditedRequestEvent message)
    {
        Changes = true;
    }
}
