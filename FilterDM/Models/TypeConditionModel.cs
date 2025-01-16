using System.Collections.Generic;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;

public class TypeConditionModel : ConditionModel
{
    public List<string> SelectedTypes { get; set; }

    public TypeConditionModel()
    {
        SelectedTypes = [];
    }

    public bool HasType(string type)
    {
        foreach (string selectedType in SelectedTypes)
        {
            if (selectedType.Equals(type))
            {
                return true;
            }
        }
        return false;
    }

    public void Add(string item) => SelectedTypes.Add(item);

    public void Remove(string item)
    {
        if (SelectedTypes.Contains(item))
        {
            SelectedTypes.Remove(item);
        }
    }

    public TypeConditionModel Clone()
    {
        TypeConditionModel clone = new();
        if (SelectedTypes != null)
        {
            clone.SelectedTypes = [.. SelectedTypes];
        }
        return clone;
    }

    internal void AddRange(List<string> list)
    {
        SelectedTypes.AddRange(list);
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
