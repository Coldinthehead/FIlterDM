using System.Collections.Generic;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;

public class ClassConditionModel : ConditionModel
{
    public List<string> SelectedClasses { get; set; }


    public ClassConditionModel()
    {
        SelectedClasses = [];
    }

    internal bool HasClass(string name)
    {
        foreach (string selectedName in  SelectedClasses)
        {
            if (name.Equals(selectedName))
            {
                return true;
            }
        }
        return false;
    }

    internal void Remove(string name)
    {
        if (SelectedClasses.Contains(name))
        {
            SelectedClasses.Remove(name);
        }
    }

    internal void Add(string item) => SelectedClasses.Add(item);
    public ClassConditionModel Clone()
    {
        ClassConditionModel clone = new();
        clone.SelectedClasses = [.. SelectedClasses];
        return clone;
    }

    internal void AddRange(IEnumerable<string> list)
    {
        SelectedClasses.AddRange(list);
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
