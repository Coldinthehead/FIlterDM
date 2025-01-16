#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;

public class RarityConditionModel : ConditionModel
{
    public bool UseNormal { get; set; }
    public bool UseMagic { get; set; }
    public bool UseRare { get; set; }
    public bool UseUnique { get; set; }

    internal string GetTitle()
    {
        string res = "";
        if (UseNormal)
        {
            res += "N ";
        }
        if (UseMagic)
        {
            res += "M ";
        }
        if (UseRare)
        {
            res += "R ";
        }
        if (UseUnique)
        {
            res += "U";
        }

        return res;
    }

    internal void UseRarity(string value)
    {
        if (value.Equals("Normal"))
        {
            UseNormal = true;
        }
        else if (value.Equals("Magic"))
        {
            UseMagic = true;
        }
        else if (value.Equals("Rare"))
        {
            UseRare = true;
        }
        else if (value.Equals("Unique"))
        {
            UseUnique = true;
        }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
