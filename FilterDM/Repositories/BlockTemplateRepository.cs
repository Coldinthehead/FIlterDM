﻿using FilterDM.Models;
using FilterDM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Repositories;

public class BlockTemplateRepository : IBlockTemplateRepository
{
    private Dictionary<string, BlockModel> _templates;

    private ObservableCollection<string> _templateNames;

    const string REPOS_PATH = "./data/templates/templates.json";

    private BlockModel _empty;

    public BlockTemplateRepository()
    {
        _templates = new();
        _empty = new()
        {
            Enabled = true,
            Priority = 2000,
            TemplateName = "Empty",
        };
        _templates["Empty"] = _empty;
        _templateNames = new([.. _templates.Keys]);
    }
    public async Task Init()
    {
        _templates = new();
        try
        {
            using var fs = File.OpenRead(REPOS_PATH);
            var items = await JsonSerializer.DeserializeAsync<Dictionary<string, BlockModel>>(fs);
            if (items != null)
            {
                _templates = items;
            }

        }
        catch (Exception ex)
        {

        }

        if (_templates.ContainsKey("Empty"))
        {
            _empty = _templates["Empty"];
        }
        else
        {
            _empty = new()
            {
                Enabled = true,
                Priority = 2000,
                TemplateName = "Empty",
            };
            _templates["Empty"] = _empty;
        }

        _templateNames = new([.. _templates.Keys]);
    }

    public BlockModel GetEmpty() => _empty.Clone();
    internal IEnumerable<string> GetTempalteNames()
    {
        return [.. _templates.Keys];
    }

    internal BlockModel? GetTemplate(string name)
    {
        if (_templates.TryGetValue(name, out BlockModel template))
        {
            return template.Clone();
        }
        return null;
    }

    internal ObservableCollection<string> GetObservableNames()
    {
        return _templateNames;
    }
}
