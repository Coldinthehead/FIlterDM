using FilterDM.Models;
using FilterDM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Repositories;

public class RuleTemplateRepository : IRuleTemplateRepository
{
    public Dictionary<string, RuleModel> _templates;
    private RuleModel _empty;
    private readonly IPersistentDataService _dataService;

    private const string DEFAULT_TEMPLATES_FILENAME = "defaults.json";

    public RuleTemplateRepository(IPersistentDataService dataService)
    {
        _templates = new Dictionary<string, RuleModel>();
        _empty = new()
        {
            Title = "Empty",
            Enabled = true,
            Show = true,
            TemplateName = "Empty",
            Priority = 2000,

        };
        _templates["Empty"] = _empty;
        _dataService = dataService;
    }

    public RuleModel GetEmpty()
    {
        return _empty.Clone();
    }

    public async Task Init()
    {
        try
        {
            string path = Path.Combine(_dataService.RuleTemaplatesPath, DEFAULT_TEMPLATES_FILENAME);
            if (!File.Exists(path))
            {
                using var ws = File.Create(path);
                Dictionary<string, RuleModel> defaultTempaltes = [];
                defaultTempaltes.Add("Empty", new()
                {
                    Title = "Empty",
                    Enabled = true,
                    Show = true,
                    TemplateName = "Empty",
                    Priority = 2000,

                });
                await JsonSerializer.SerializeAsync<Dictionary<string, RuleModel>>(ws, defaultTempaltes);
                ws.Close();
            }

            using var fs = File.OpenRead(path);
            var items = await JsonSerializer.DeserializeAsync<Dictionary<string, RuleModel>>(fs);
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
    public IEnumerable<RuleModel> GetAll() => [.. _templates.Values];
    public RuleModel Get(string templateName) => _templates[templateName].Clone();
    public bool Has(string templateName) => _templates.ContainsKey(templateName);
}
