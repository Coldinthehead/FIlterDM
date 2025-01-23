using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
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
        IStorageFile? outFile = await _fileSelectionService.ExportFilterFile(_currentFilterVm.Name);
        if (outFile != null)
        {
            FilterModel model = _currentFilterVm.GetModel();
            string output = _exportService.Build(model);
            await _fileService.SavePoeFile(outFile, output);
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
            IStorageFile? file = await _fileSelectionService.OpenPoeFile();
            if (file == null)
            {
                return;
            }

            string input = File.ReadAllText(file.Path.LocalPath);
            var result = App.Current.Services.GetService<FilterParserService>().Parse(input);
            if (result.Errors.Count  > 0)
            {
                await App.Current.Services.GetService<DialogService>().ShowOkDialog($"Import Error: {result.Errors.First()}");
            }
            var model = result.Model;
            model.Name = Path.GetFileNameWithoutExtension(file.Path.LocalPath);
            OnEnter(model);
            await App.Current.Services.GetService<DialogService>().ShowOkDialog($"Filter imported with {result.TotalRules} Rules");
        }
        catch (Exception e)
        {
        }
    }

    public void OnEnter(FilterModel model)
    {
        Name = model.Name;
        _currentFilterVm = new(_typeService, _blockTemplateService, _ruleTemplateService, _iconService);
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
    private readonly FilterExportService _exportService;
    private readonly MinimapIconsService _iconService;

    public ProjectEditViewModel(ItemTypeService typeService
        , BlockTemplateService blockTempalteService
        , RuleTemplateService ruleTempalateService
        , IProjectService projectService
        , FileSelectionService fileSelectionService
        , FileService fileService
        , FilterExportService exportService
        , MinimapIconsService iconService)
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
        _exportService = exportService;
        _iconService = iconService;
    }

    public void Receive(FilterEditedRequestEvent message)
    {
        Changes = true;
    }
}
