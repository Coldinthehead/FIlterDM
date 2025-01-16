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
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.ViewModels.Pages;


public partial class ProjectEditViewModel : ObservableRecipient
    , IRecipient<BlockSelectedRequestEvent>
    , IRecipient<BlockCloseRequestEvent>
    , IRecipient<BlockDeleteRequestEvent>
    , IRecipient<RuleSelectedRequestEvent>
    , IRecipient<RuleCloseRequestEvent>
    , IRecipient<RuleDeleteRequestEvent>
    , IRecipient<RuleCreateRequestEvent>
    , IRecipient<BlockCreatedRequestEvent>
    , IRecipient<BlockPriorityChangedRequest>
    , IRecipient<FilterEditedRequestEvent>
    , IRecipient<BlockModelChangedEvent>
{

    [ObservableProperty]
    private StructureTreeViewModel _filterTree;
    [ObservableProperty]
    private EditorPanelViewModel _editorPanel;

    [ObservableProperty]
    private string _name;

    private string _lastFilename;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCurrentCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsCommand))]
    private bool _changes = false;
    public bool CanSave()
    {
        return Changes;
    }

    public bool CanSaveAs()
    {
        return Changes;
    }

    public FilterModel Model => _model;
    private FilterModel _model;


    public Action BackToMenuAction { get; set; }
    [RelayCommand]
    private async void NewFilter()
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
    private async void Export()
    {
        try
        {
            var filesService = App.Current?.Services?.GetService<FilesService>();
            var file = await filesService.ExportFilterFile(_model.Name);
            if (file != null)
            {
                var str = App.Current.Services.GetService<CoreFilterService>().Build(_model);
                File.WriteAllText(file.Path.LocalPath, str);
                await App.Current.Services.GetService<DialogService>().ShowOkDialog("Filter Exported!");
            }
        }
        catch (Exception ex)
        {

        }

    }

    [RelayCommand(CanExecute = nameof(CanSaveAs))]
    private async void SaveAs()
    {
        try
        {
            var filesService = App.Current?.Services?.GetService<FilesService>();
            var file = await filesService.SaveFilterProjectFile(_model.Name);

            await using var writeStream = File.Create(file.Path.LocalPath);
            var opt = new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            JsonSerializer.Serialize(writeStream, _model, options: opt);
            if (file is null)
                return;
            else
            {
                Name = Path.GetFileNameWithoutExtension(file.Name);
                _model.Name = Name;
                _lastFilename = file.Path.LocalPath;

            }
            Changes = false;

        }
        catch (Exception e)
        {

        }
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async void SaveCurrent()
    {
        try
        {
            await App.Current.Services.GetService<ProjectRepositoryService>().SaveFilter(_model);
            _ = await App.Current.Services.GetService<DialogService>().ShowOkDialog($"Filter {_model.Name} saved!");
            Changes = false;
        }
        catch (Exception ex)
        {

        }

    }

    [RelayCommand]
    private async void Load()
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
            var filesService = App.Current?.Services?.GetService<FilesService>();
            if (filesService is null)
                throw new NullReferenceException("Missing File Service instance.");

            var file = await filesService.OpenFilterProjectFile();
            if (file is null)
                return;

            await using var readStream = await file.OpenReadAsync();
            FilterModel? m = await JsonSerializer.DeserializeAsync<FilterModel>(readStream);
            if (m != null)
            {
                _lastFilename = file.Path.LocalPath;
                OnEnter(m);
            }
        }
        catch (Exception e)
        {
        }
    }

    [RelayCommand]
    private async Task Parse()
    {
        try
        {
            var filesService = App.Current?.Services?.GetService<FilesService>();
            if (filesService is null)
                throw new NullReferenceException("Missing File Service instance.");

            var file = await filesService.OpenFilterProjectFile();
            if (file is null)
                return;

          /*  if (!Path.GetExtension(file.Path.LocalPath).Equals("filter"))
            {
                await App.Current.Services.GetService<DialogService>().ShowOkDialog("Select file with .filter extension!");
                    return;
            }*/

            string input = File.ReadAllText(file.Path.LocalPath);
            var model = App.Current.Services.GetService<FilterParserService>().Parse(input);
            if (model != null)
            {
                _lastFilename = file.Path.LocalPath;
                model.Name = Path.GetFileNameWithoutExtension(file.Path.LocalPath);
                OnEnter(model);
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
        _model = model;
        Name = model.Name;
        FilterTree = new(_model);
        EditorPanel = new();
        Changes = false;
        SaveCurrentCommand.NotifyCanExecuteChanged();
    }

    public ProjectEditViewModel()
    {
        Messenger.Register<BlockSelectedRequestEvent>(this);
        Messenger.Register<BlockCloseRequestEvent>(this);
        Messenger.Register<BlockDeleteRequestEvent>(this);
        Messenger.Register<BlockCreatedRequestEvent>(this);
        Messenger.Register<RuleSelectedRequestEvent>(this);
        Messenger.Register<RuleCloseRequestEvent>(this);
        Messenger.Register<RuleDeleteRequestEvent>(this);
        Messenger.Register<RuleCreateRequestEvent>(this);
        Messenger.Register<BlockPriorityChangedRequest>(this);
        Messenger.Register<FilterEditedRequestEvent>(this);
        Messenger.Register<BlockModelChangedEvent>(this);
    }

    public void Receive(BlockSelectedRequestEvent message)
    {
        EditorPanel.AddBlock(message.Value, _model);
        FilterTree.SelectedNode = message.Value;

    }

    public void Receive(BlockCloseRequestEvent message)
    {
        FilterTree.SelectedNode = null;
    }

    public void Receive(BlockDeleteRequestEvent message)
    {
        _model.DeleteBlock(message.Value.Model);
        FilterTree.RemoveBlock(message.Value);
        EditorPanel.CloseRulesFromBlock(message.Value);
    }

    public void Receive(RuleSelectedRequestEvent message)
    {
        EditorPanel.AddRule(message.Value);
        FilterTree.SelectedNode = message.Value;
    }

    public void Receive(RuleCloseRequestEvent message)
    {

    }

    public void Receive(RuleDeleteRequestEvent message)
    {
        EditorPanel.CloseRule(message.Value);
    }

    public void Receive(RuleCreateRequestEvent message)
    {
        FilterTree.SelectedNode = message.Value.RealParent;
    }

    public void Receive(BlockCreatedRequestEvent message)
    {

        FilterTree.SelectedNode = message.Value;
    }

    public void Receive(BlockPriorityChangedRequest message)
    {
        FilterTree.SortBlocks();
    }

    public void Receive(FilterEditedRequestEvent message)
    {
        Changes = true;
        SaveCurrentCommand.NotifyCanExecuteChanged();
    }

    public void Receive(BlockModelChangedEvent message)
    {
    }
}
