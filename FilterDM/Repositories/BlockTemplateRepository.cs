using FilterDM.Models;
using FilterDM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Repositories;

public class BlockTemplateRepository : IBlockTemplateRepository
{
    private Dictionary<string, BlockModel> _templates;
    private BlockModel _empty;
    private readonly PersistentDataService _dataService;

    private const string DEFAULTS_FILENAME = "defaults.json";

    public BlockTemplateRepository(PersistentDataService dataService)
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
        _dataService = dataService;
    }
    public async Task Init()
    {
        try
        {
            string path = Path.Combine(_dataService.BlockTemaplatesPath, DEFAULTS_FILENAME);
            if (!File.Exists(path))
            {
                using var ws = File.Create(path);
                Dictionary<string, BlockModel> defaults = new()
                {
                    { "Empty", _empty }
                };
                await JsonSerializer.SerializeAsync<Dictionary<string, BlockModel>>(ws, defaults);
                ws.Close();
            }
            using var fs = File.OpenRead(path);
            var items = await JsonSerializer.DeserializeAsync<Dictionary<string, BlockModel>>(fs);
            if (items != null)
            {
                _templates = items;
            }

        }
        catch (Exception ex)
        {

        }
        _empty = _templates["Empty"];
    }

    public BlockModel GetEmpty() => _empty.Clone();
    public bool Exists(string templateName)
    {
        if (templateName == null)
        {
            return false;
        }
        return _templates.ContainsKey(templateName);
    }
    public BlockModel Get(string templateName)
    {
        return _templates[templateName].Clone();
    }

    public List<BlockModel> GetAll() => [.. _templates.Values];
}
