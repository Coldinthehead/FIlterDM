using FilterCore.Conditions;
using FilterCore.Conditions.Builders;
using FilterCore.Decorations;
using FilterCore.Decorations.Builder;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace FilterCore;

public class ItemBlock
{
    public readonly List<FilteringCondition> Conditions = [];
    public readonly List<DecorationProperty> Decorations = [];

    public readonly string Name;

    public string ParentName
    {
        get => _parentName + "." + Name;
        set
        {
            _parentName = value;
            foreach( var condition in Conditions)
            {
                condition.ParentName = ParentName;
            }
        }
    }

    private string _parentName;

    public ItemBlock(string name)
    {
        Name = name;
        _parentName = string.Empty;
    }

    public ItemBlock FilterBy(FilteringCondition condition)
    {
        if (GetFilteringCondition(condition.GetType()) != null)
        {
            throw new ArgumentException($"Multiple declaration of [{condition.Name}] for {Name}");
        }
        Conditions.Add(condition);
        condition.ParentName = ParentName;
        return this;
    }

    public ItemBlock FilterBy(IConditionBuilder builder)
    {
        Conditions.AddRange(builder.Apply());
        return this;
    }


    public ItemBlock AddDecoration(DecorationProperty property)
    {
        if (Decorations.Contains(property))
        {
            throw new ArgumentException($"Decoration [{property.Content}] already applied for {Name}");
        }
        Decorations.Add(property);
        return this;
    }

    public ItemBlock AddDecoration(params DecorationProperty[] items)
    {
        foreach (var item in items)
        {
            AddDecoration(item);
        }
        return this;
    }

    public ItemBlock AddDecoration(DecorationBuilder builder)
    {
        foreach (var item in builder.Build())
        {
            AddDecoration(item);
        }
        return this;
    }

    public T GetFilteringCondition<T>() where T: FilteringCondition
    {
        foreach (FilteringCondition condition in Conditions)
        {
            if (condition is T insance)
            {
                return condition as T;
            }
        }
        return null;
    }

    public FilteringCondition GetFilteringCondition(Type type)
    {
        foreach (FilteringCondition condition in Conditions)
        {
            if (condition.GetType() == type)
            {
                return condition;
            }
        }
        return null;
    }

    public string DumpFilterString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Show # block : {Name}");
        foreach(FilteringCondition condition in Conditions)
        {
            sb.AppendLine(condition.DumpSting());
        }
        foreach (DecorationProperty property in Decorations)
        {
            sb.AppendLine($"{Helper.Tab}{property.Content}");
        }

        return sb.ToString();
    }
}
