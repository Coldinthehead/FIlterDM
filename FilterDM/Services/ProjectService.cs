using FilterDM.Models;
using FilterDM.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Channels;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FilterDM.Services;
public class ProjectService : IProjectService
{
    private readonly ProjectRepository _repository;
    private readonly DialogService _dialogService;
    public ProjectService(ProjectRepository repository, DialogService dialogService)
    {
        _repository = repository;
        _dialogService = dialogService;
    }

    public void Add(FilterModel model)
    {
        if (_repository.Has(model.Name))
        {
            model.Name = _repository.GetFreeName(model.Name);
        }
        _repository.CreateFilter(model);
    }

    public FilterModel Get(string filterName) => _repository.GetProjectModel(filterName);
    public List<string> Names() => _repository.GetProjectNames;

    public async Task<bool> Save(FilterModel filterModel)
    {
        try
        {
            await _repository.SaveFilter(filterModel);
            _ = await _dialogService.ShowOkDialog($"Filter {filterModel.Name} saved!");
            return true;
        }
        catch (Exception ex)
        {
            await _dialogService.ShowOkDialog($"Error has occured! Filter not saved.");
        }
        return false;
    }
}

public interface IProjectService
{
    void Add(FilterModel model);
    FilterModel Get(string filterName);
    List<string> Names();
    Task<bool> Save(FilterModel filterModel);
}
