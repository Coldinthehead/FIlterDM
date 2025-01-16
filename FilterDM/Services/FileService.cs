using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class FilesService 
{
    private readonly Window _target;

    public FilesService(Window target)
    {
        _target = target;
    }

    public async Task<IStorageFile?> OpenFilterProjectFile()
    {
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync("./data/filters");
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Filter File",
            AllowMultiple = false,
            SuggestedStartLocation = storageFolder,
        });

        return files.Count >= 1 ? files[0] : null;
    }

    public async Task<IStorageFile?> SaveFilterProjectFile(string filterName)
    {
        IStorageFolder? storageFolder = await _target.StorageProvider.TryGetFolderFromPathAsync("./data/filters");
        return await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            
            Title = "Save Filter File",
            DefaultExtension =".json",
            SuggestedFileName = $"{filterName}.json",
            FileTypeChoices = [FilePickerFileTypes.All],
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