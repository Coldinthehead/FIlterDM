using FilterDM.Models;
using FilterDM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilterDM.Repositories;

public class ProjectRepository : IInit
{
    private Dictionary<string, FilterModel> _models;

    private readonly FileService _fileService;

    private readonly IPersistentDataService _dataService;

    public ProjectRepository(FileService fileService, IPersistentDataService dataService)
    {
        _fileService = fileService;
        _models = [];
        _dataService = dataService;
    }

    public async Task Init()
    {
        var filters = Directory.GetFiles(_dataService.FiltersPath);
        _models = new(filters.Length);
        var tasks = filters.Select(async fname =>
        {
            FilterModel? model = await LoadModelFromFileAsync(fname);
            if (model != null)
            {
                _models[model.Name] = model;
            }
        });
        await Task.WhenAll(tasks);
    }

    public List<string> GetProjectNames => [.. _models.Keys.OrderByDescending(x=> _models[x].LastSaveDate)];

    internal void CreateFilter(FilterModel model)
    {
        _models.Add(model.Name, model);
        Task.Run(() => SaveFilter(model));
    }
    internal FilterModel? GetProjectModel(string filterName)
    {
        if (_models.ContainsKey(filterName))
        {
            return _models[filterName];
        }
        else
        {
            return null;
        }
    }

    public async Task SaveFilter(FilterModel model)
    {
        model.LastSaveDate = DateTime.Now;
        _models[model.Name] = model;
        await _fileService.Save(model, GetJsonAbsolutePath(model.Name));
    }
    internal void DeleteByName(string title)
    {
        if (_models.ContainsKey(title))
        {
            _fileService.DeleteFile(GetJsonAbsolutePath(title));
        }
    }

    private async Task<FilterModel?> LoadModelFromFileAsync(string filePath)
    {
        return await _fileService.LoadProject(filePath);
    }

    public bool Has(string name)
    {
        return _models.ContainsKey(name);
    }

    internal string GetFreeName(string name)
    {
        int i = 1;
        string title = $"{name}({1})";
        while (Has(title))
        {
            title = $"{name}({i++})";
        }
        return title;
    }
    private string GetJsonAbsolutePath(string filterName)
    {
        var filename = Path.Combine(_dataService.FiltersPath, filterName);
        filename = Path.ChangeExtension(filename, "json");
        return filename;
    }
}
