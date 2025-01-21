
using Avalonia.Platform.Storage;
using FilterDM.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;
public class FileService
{
    private JsonSerializerOptions _jsonOpt = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };


    private readonly DialogService _dialogService;

    public FileService(DialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public async Task<bool> Save(IStorageFile file, FilterModel filterModel)
    {
        try
        {
            using var fs = File.Create(file.Path.LocalPath);
            await JsonSerializer.SerializeAsync(fs, filterModel, _jsonOpt);
            await _dialogService.ShowOkDialog($"Project {filterModel} saved!");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowOkDialog("Error has occured! Project not saved.");
            return false;
        }
        return true;
    }

    public async Task<FilterModel> LoadProject(IStorageFile file)
    {
        try
        {
            using var fs = File.OpenRead(file.Path.LocalPath);
            FilterModel? model = await JsonSerializer.DeserializeAsync<FilterModel>(fs, _jsonOpt);
            return model ?? throw new ArgumentNullException($"Failed to parse {file.Path.LocalPath} file");
        }
        catch
        {
            await _dialogService.ShowOkDialog("Error has occured! Project not loaded.");
            throw;
        }
    }
}
