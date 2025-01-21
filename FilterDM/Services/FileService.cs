
using Avalonia.Platform.Storage;
using FilterDM.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            filterModel.Name = Path.GetFileNameWithoutExtension(file.Name);
            await JsonSerializer.SerializeAsync(fs, filterModel, _jsonOpt);
            await _dialogService.ShowOkDialog($"Project {filterModel.Name} saved!");
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

    public async Task Save(FilterModel model, string path)
    {
        using var fs = File.Create(path);
        await JsonSerializer.SerializeAsync(fs, model, _jsonOpt);
    }

    public async Task<FilterModel?> LoadProject(string filePath)
    {
        try
        {
            using var fs = File.OpenRead(filePath);
            var result = await JsonSerializer.DeserializeAsync<FilterModel>(fs, _jsonOpt);
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        return null;
    }

    public void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
