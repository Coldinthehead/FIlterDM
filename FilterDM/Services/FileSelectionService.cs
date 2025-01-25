using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class FileSelectionService 
{
    private readonly Window _target;
    private readonly IPersistentDataService _dataService;

    private static FilePickerFileType JsonFileChoice { get; } = new FilePickerFileType("JSON File")
    {
        Patterns = new[] { "*.json" },
        AppleUniformTypeIdentifiers = new[] { "public.json" },
        MimeTypes = new[] { "application/json" }
    };

    private static FilePickerFileType FilterFileChoice { get; } = new FilePickerFileType("Filter File")
    {
        Patterns = new[] { "*.filter" },
        AppleUniformTypeIdentifiers = new[] { "public.filter" },
        MimeTypes = new[] { "PathOfExileFilter/filter" }
    };


    public FileSelectionService(Window target, IPersistentDataService dataService)
    {
        _target = target;
        _dataService = dataService;
    }

    public async Task<IStorageFile?> OpenProjectFile()
    {
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync(_dataService.FiltersPath);
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Filter File",
            AllowMultiple = false,
            FileTypeFilter = [JsonFileChoice],
            SuggestedStartLocation = storageFolder,
        });

        return files.Count >= 1 ? files[0] : null;
    }

    public async Task<IStorageFile?> SaveProjectFile(string projectName)
    {
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync(_dataService.FiltersPath);
        return await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            
            Title = "Save Filter File",
            DefaultExtension =".json",
            SuggestedFileName = $"{projectName}.json",
            FileTypeChoices = [JsonFileChoice],
            SuggestedStartLocation = storageFolder
        });
    }

    public async Task<IStorageFile?> ExportFilterFile(string name)
    {
        return await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {

            Title = "Export Filter File",
            SuggestedFileName = $"{name}.filter",
            FileTypeChoices = [FilterFileChoice],
        });
    }

    public async Task<IStorageFile?> OpenPoeFile()
    {
        string poeFilterPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Path of Exile 2");
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync(poeFilterPath);
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Filter File",
            AllowMultiple = false,
            FileTypeFilter = [FilterFileChoice],
            SuggestedStartLocation = storageFolder,
        });

        return files.Count >= 1 ? files[0] : null;
    }

}
