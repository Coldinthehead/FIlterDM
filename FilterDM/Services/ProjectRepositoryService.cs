using FilterDM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class ProjectRepositoryService : IInit
{
    private const string RepositoryPath = "./data/filters";

    private Dictionary<string, FilterModel> _models;

    public async Task Init()
    {
        var filters = Directory.GetFiles(RepositoryPath);
        _models = new(filters.Length);
        var tasks = filters.Select(async fname =>
        {
            FilterModel? model = await LoadModelFromFileAsync(fname);
            if (model != null)
            {
                _models[model.Name]=  model;
            }
        });
        Task.WaitAll(tasks);
    }

    public List<string> GetProjectNames => [.. _models.Keys];

    internal void CreateFilter(FilterModel model)
    {
        _models.Add(model.Name, model);
        Task.Run(()=>SaveFilter(model));
    }

    public async Task SaveFilter(FilterModel model)
    {
        var filename = GetFullName(model.Name);
        using var fs = File.Create(filename);
        var opt = new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
        await JsonSerializer.SerializeAsync<FilterModel>(fs, model, opt);
    }

    private async Task<FilterModel> LoadModelFromFileAsync(string filePath)
    {
        try
        {
            using var fs = File.OpenRead(filePath);
            var result = await JsonSerializer.DeserializeAsync<FilterModel>(fs);
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

    internal void DeleteByName(string title)
    {
        if (_models.ContainsKey(title))
        {
            var name = GetFullName(title);
            if (File.Exists(name))
            {
                File.Delete(name);
            }
        }
    }

    private string GetFullName(string filterName)
    {
        var filename = Path.Combine(RepositoryPath, filterName);
        filename = Path.ChangeExtension(filename, "json");
        return filename;
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
}
