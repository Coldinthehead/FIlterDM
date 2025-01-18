
using FilterCore.PoeData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class ItemClassesService : IItemClassesService
{
    private List<ItemClassDetails> _classes;

    private const string REPO_PATH = "./data/poe/classes.json";

    public async Task Init()
    {
        using var fs = File.OpenRead(REPO_PATH);
        List<ItemClassDetails> classes = await JsonSerializer.DeserializeAsync<List<ItemClassDetails>>(fs);
        if (classes != null)
        {
            _classes = [.. classes.OrderBy(x=> x.Name)];
            return;
        }
        else
        {
            throw new FileNotFoundException($"Cannot find class repo at '{REPO_PATH}'");
        }
    }

    public List<ItemClassDetails> GetItemClasses()
    {
        return [.. _classes];
    }

    internal IEnumerable<string> GetPartialMatches(string partialvalue)
    {
        List<string> result = [];
        foreach (var item in _classes)
        {
            if (item.Name.StartsWith(partialvalue) || item.Name.Contains(partialvalue))
            {
                result.Add(item.Name);
            }
        }

        return result;
    }
    public IEnumerable<string> GetExactMatches(string name)
    {
        List<string> res = [];
        foreach (var item in _classes)
        {
            if (item.Name.Equals(name))
            {
                res.Add(item.Name);
            }
        }
        return res;
    }
}
