
using FilterCore;
using FilterDM.ViewModels.EditPage.Decorators;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilterDM.Services;

public class ItemTypeService : IItemClassesService
{
    Dictionary<string, List<ItemTypeDetails>> _typeCategories = [];

    private const string REPO_PATH = "./data/poe/types.json";

    public async Task Init()
    {
        using var fs = File.OpenRead(REPO_PATH);
        Dictionary<string, List<ItemTypeDetails>> types = await JsonSerializer.DeserializeAsync<Dictionary<string, List<ItemTypeDetails>>> (fs);
        if (types != null)
        {
            _typeCategories = types;
        }
        else
        {
            throw new FileNotFoundException($"Cannot find type repo at '{REPO_PATH}'");
        }
    }

    public Dictionary<string, List<ItemTypeDetails>> GetItemTypes() => _typeCategories;
    internal IEnumerable<string> GetPartialMatches(string v)
    {
        List<string> result = [];
        foreach (List<ItemTypeDetails> category in _typeCategories.Values)
        {
            foreach (ItemTypeDetails item in category)
            {
                if (item.Name.StartsWith(v) || item.Name.Contains(v))
                {
                    result.Add(item.Name);
                }
            }
        }

        return result;
    }

    public IEnumerable<string> GetExactMatches(string name)
    {
        List<string> result = [];
        foreach (var category in _typeCategories.Values)
        {
            foreach (ItemTypeDetails item in category)
            {
                if (item.Name.Equals(name))
                {
                    result.Add(item.Name);  
                }
            }
        }

        return result;
    }

    public List<TypeListViewModel> BuildEmptyList()
    {
        List<TypeListViewModel> catagoryList = [];
        foreach (string currentCategoryName in _typeCategories.Keys)
        {
            TypeListViewModel listViewModel = new();
            listViewModel.Title = currentCategoryName;
            List<TypeViewModel> itemNamesList = [];
            foreach (ItemTypeDetails typeDetails in  _typeCategories[currentCategoryName])
            {
                TypeViewModel typeModel = new()
                {
                    Name = typeDetails.Name,
                    Description = typeDetails.Tip != string.Empty ? typeDetails.Tip : "No Tip :(",
                };
                itemNamesList.Add(typeModel);
            }
            listViewModel.Types = new(itemNamesList);
            catagoryList.Add(listViewModel);
        }
        return catagoryList;
    }
}
