using Avalonia.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;
public class FilterModel
{
    public string Name { get; set; }
    public Guid ID { get; set; }    
    public DateTime LastSaveDate { get; set; }
    public List<BlockModel> Blocks { get; set; }

    public FilterModel()
    {
        Blocks = new List<BlockModel>();
    }

    public BlockModel AddBlock(string title)
    {
        var block = new BlockModel()
        {
            Title = title,
        };
        Blocks.Add(block);
        return block;
    }

    public BlockModel AddBlock(BlockModel m)
    {
        Blocks.Add(m);
        return m;
    }

    public BlockModel DeleteBlock(BlockModel block)
    {
        Blocks.Remove(block);
        return block;
    }

 


}

public class BlockModel : IEquatable<BlockModel>
{
    public string Title { get; set; }
    public bool Enabled { get; set; }
    public float Priority { get; set; }
    public List<RuleModel> Rules { get; set; }
    public string TemplateName { get; set; }

    public bool UseBlockTypeScope { get; set; }

    public BlockModel()
    {
        Rules = [];
    }

    public void AddRule(RuleModel nextTempate)
    {
        Rules.Add(nextTempate);
    }


    public void DeleteRule(RuleModel rule)
    {
        Rules.Remove(rule);
    }
    public string GetGenericRuleTitle()
    {
        int i = 1;
        string title = "Rule(0)";

        while (RuleTitleTaken(title))
        {
            title = $"Rule({i++})";
        }
        return title;
    }

    private bool RuleTitleTaken(string title)
        => Rules.Select(x => x.Title).Any((t) => string.Equals(title, t));

    public bool Equals(BlockModel? other) => this == other;
    internal BlockModel Clone()
    {
        BlockModel clone = new();
        clone.Priority = Priority;
        clone.Enabled = Enabled;
        clone.Title = Title;
        foreach (var rule in Rules)
        {
            clone.AddRule(rule.Clone());    
        }
        if (TemplateName != null)
        {
            clone.TemplateName = TemplateName;
        }

        return clone;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
