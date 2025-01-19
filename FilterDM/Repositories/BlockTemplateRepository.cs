using FilterDM.Models;
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
            Title = "Empty",
        };
        _templates["Empty"] = _empty;
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
                Title = "Empty",
            };
            _templates["Empty"] = _empty;
        }
    }

    public BlockModel GetEmpty() => _empty.Clone();
    public bool Exists(string templateName)
    {
        return _templates.ContainsKey(templateName);
    }
    public BlockModel Get(string templateName)
    {
        return _templates[templateName];
    }

    public List<BlockModel> GetAll() => [.. _templates.Values];
}
