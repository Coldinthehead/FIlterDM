using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class FileSelectionService 
{
    private readonly Window _target;

    public static FilePickerFileType JsonFileChoice { get; } = new FilePickerFileType("JSON File")
    {
        Patterns = new[] { "*.json" },
        AppleUniformTypeIdentifiers = new[] { "public.json" },
        MimeTypes = new[] { "application/json" }
    };


    public FileSelectionService(Window target)
    {
        _target = target;
    }

    public async Task<IStorageFile?> OpenProjectFile()
    {
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync("./data/filters");
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
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync("./data/filters");
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
            FileTypeChoices = [FilePickerFileTypes.All],
        });
    }

}
