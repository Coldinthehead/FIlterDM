﻿using FilterDM.Models;
using FilterDM.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Channels;
using System;
using System.Threading.Tasks;

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
    Task<bool> Save(FilterModel filterModel);
}
