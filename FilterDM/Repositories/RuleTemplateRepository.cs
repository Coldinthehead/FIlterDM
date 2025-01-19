using FilterDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Repositories;

public class RuleTemplateRepository : IRuleTemplateRepository
{
    public Dictionary<string, RuleModel> _templates;
    const string REPOS_PATH = "./data/templates/rules.json";
    private RuleModel _empty;

    public RuleTemplateRepository()
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
    }

    public RuleModel GetEmpty()
    {
        return _empty.Clone();
    }

    public async Task Init()
    {
        _templates = new();
        try
        {
            using var fs = File.OpenRead(REPOS_PATH);
            var items = await JsonSerializer.DeserializeAsync<Dictionary<string, RuleModel>>(fs);
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
                Title = "Empty",
                Enabled = true,
                Show = true,
                TemplateName = "Empty",
                Priority = 2000,

            };
            _templates["Empty"] = _empty;
        }
    }
    public IEnumerable<RuleModel> GetAll() => [.. _templates.Values];
    public RuleModel Get(string templateName) => _templates[templateName].Clone();
    public bool Has(string templateName) => _templates.ContainsKey(templateName);
}
