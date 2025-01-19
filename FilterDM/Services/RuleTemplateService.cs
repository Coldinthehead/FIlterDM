using FilterDM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class RuleTemplateService : IInit
{
    public Dictionary<string, RuleModel> _templates;

    private ObservableCollection<string> _templateNames;

    const string REPOS_PATH = "./data/templates/rules.json";

    private RuleModel _empty;

    public RuleTemplateService()
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
        _templateNames = new(_templates.Keys);
    }

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
        _templateNames = new(_templates.Keys);
    }

    public RuleModel? GetTemplate(string selectedTemplate)
    {
        if (_templates.TryGetValue(selectedTemplate, out RuleModel? tempalte))
        {
            return tempalte.Clone();
        }
        return null;
    }

    public ObservableCollection<string> GetObservableNames() => _templateNames;
}
