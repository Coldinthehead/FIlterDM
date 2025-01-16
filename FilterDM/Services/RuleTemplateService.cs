using FilterDM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class BlockTemplateService : IInit
{
    private Dictionary<string, BlockModel> _templates;

    const string REPOS_PATH = "./data/templates/templates.json";

    private BlockModel _empty;
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

            };
            _templates["Empty"] = _empty;
        }
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
}

public class RuleTemplateService : IInit
{
    public Dictionary<string, RuleModel> _templates;

    const string REPOS_PATH = "./data/templates/rules.json";

    private RuleModel _empty;

    public RuleModel Build(string name)
    {
        if (_templates.TryGetValue(name, out RuleModel? tempalte))
        {
            return tempalte.Clone();
        }
        return _empty.Clone();
    }

    public List<string> GetTempalteNames()
    {
        return [.. _templates.Keys];
    }

    public RuleModel BuildEmpty()
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

    public RuleModel? GetTemplate(string selectedTemplate)
    {
        if (_templates.TryGetValue(selectedTemplate, out RuleModel? tempalte))
        {
            return tempalte.Clone();
        }
        return null;
    }
}
