using Avalonia.Animation;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Tmds.DBus.Protocol;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace FilterDM.Models;

public class RuleModel : IEquatable<RuleModel>
{
    public string Title { get; set; }
    public bool Enabled { get; set; }
    public float Priority { get; set; }
    public int FontSize { get; set; }
    public bool Show { get; set; }

    public string TemplateName { get; set; }

    public ClassConditionModel? ClassCondition { get; set; }

    public TypeConditionModel? TypeCondition { get; set; }
    public RarityConditionModel RarityCondition { get; set; }
    public List<NumericCondition> NumericConditions { get; set; }
    public Dictionary<string, SerializableColor> Colors { get; set; }

    public BeamDetails? Beam { get; set; }

    public MinimapIconDetails? Icon { get; set; }

    public SoundDetails? Sound { get; set; }
    public StateModifiers StateModifiers { get; internal set; }

    public RuleModel()
    {
        NumericConditions = [];
        Colors = [];
    }

    public RuleModel Clone()
    {
        RuleModel Clone = new();

        Clone.Title = Title;
        Clone.Enabled = Enabled;
        Clone.Priority = Priority;
        Clone.FontSize = FontSize;
        Clone.Show = Show;
        
        if (ClassCondition != null)
        {
            Clone.ClassCondition = ClassCondition.Clone();
        }
        if (TypeCondition != null)
        {
            Clone.TypeCondition = TypeCondition.Clone();
        }
        Clone.NumericConditions = [.. NumericConditions.Select(x => x.Clone())];
        foreach (KeyValuePair<string, SerializableColor> pair in Colors)
        {
            Clone.Colors[pair.Key] = pair.Value.Clone();
        }
        if (Beam != null)
        {
            Clone.Beam = Beam.Clone();
        }
        if (Icon != null)
        {
            Clone.Icon = Icon.Clone();
        }
        if (Sound != null)
        {
            Clone.Sound = Sound.Clone();
        }
        if (TemplateName != null )
        {
            Clone.TemplateName = TemplateName;
        }


        return Clone;
    }

    public void SetTittle(string title)
    {
        Title = title;
    }

    public void AddTextColor(Color color)
        => Colors["TextColor"] = SerializableColor.FromAvalonia(color);

    public void RemoveTextColor()
        => Colors.Remove("TextColor");

    public bool HasAnyColor()
    {
        return Colors.Count > 0;
    }

    public bool TryGetTextColor(out Color color)
    {
        if (Colors.TryGetValue("TextColor", out var item))
        {
            color = item.ToAvalonia();
            return true;
        }
        color = Avalonia.Media.Colors.Transparent;
        return false;
    }


    public void AddBorderColor(Color color)
        => Colors["BorderColor"] = SerializableColor.FromAvalonia(color);

    public void RemoveBroderColor()
        => Colors.Remove("BorderColor");


    public bool TryGetBorderColor(out Color color)
    {
        if (Colors.TryGetValue("BorderColor", out var item))
        {
            color = item.ToAvalonia();
            return true;
        }
        color = Avalonia.Media.Colors.Transparent;
        return false;
    }
    public void AddBackgroundColor(Color color)
        => Colors["BG"] = SerializableColor.FromAvalonia(color);

    public void RemoveBackgroundColor()
        => Colors.Remove("BG");

    public bool TryGetBackgroundColor(out Color color)
    {
        if (Colors.TryGetValue("BG", out var item))
        {
            color = item.ToAvalonia();
            return true;
        }
        color = Avalonia.Media.Colors.Transparent;
        return false;
    }

    public void EnableBeam()
    {
        Beam = new();
        Beam.Color = "Red";
    }

    public void DisableBeam()
    {
        Beam = null;
    }

    public void SetBeamColor(string color)
    {
        if (Beam != null)
        {
            Beam.Color = color;
        }
    }

    public void SetBeamLifetime(bool val)
    {
        if (Beam != null)
        {
            Beam.IsPermanent = val;
        }
    }

    public void EnableIcon()
    {
        Icon = new()
        {
            Color = "Red",
            Shape = "Circle",
            Size = "Small",
        };
    }

    public void DisableIcon()
    {
        Icon = null;
    }

    public void SetIconSize(string size)
    {
        if (Icon != null)
        {
          Icon.Size = size;
        }
    }

    public void SetIconShape(string shape)
    {
        if( Icon != null)
        {
            Icon.Shape = shape;
        }
    }

    public void SetIconColor(string color)
    {
        if (Icon != null)
        {
           Icon.Color = color;
        }
    }

    public void EnableSound()
    {
        Sound = new()
        {
            Sample = 1,
            Volume = 300,
        };
    }


    public void DisableSound()
    {
        Sound = null;
    }

    public void SetSoundSample(int sample)
    {
        if (Sound != null)
        {
            Sound.Sample = sample;
        }
    }


    public void SetSoundVolume(int vol)
    {
        if (Sound != null)
        {
            Sound.Volume = vol;
        }
    }

    public ClassConditionModel AddClassCondition()
    {
        ClassConditionModel m = new();
        ClassCondition = m;
        return m;
    }

    public bool TryGetClassCondition(out ClassConditionModel? condition)
    {
        if (ClassCondition != null)
        {
            condition = ClassCondition;
            return true;
        }
        condition = null;
        return false;
    }

    public RarityConditionModel AddRarityCondition()
    {
        RarityConditionModel m = new();
        RarityCondition = m;
        return m;
    }

    public bool TryGetRarityCondition(out RarityConditionModel? condition)
    {
        if (RarityCondition != null)
        {
            condition = RarityCondition;
            return true;
        }
        condition = null;
        return false;
    }

    public NumericCondition AddNumericCondition()
    {
        NumericCondition m = new();
        NumericConditions.Add(m);
        return m;
    }
    public void AddNumericCondition(NumericCondition condition)
    {
        NumericConditions.Add(condition);
    }

    public List<NumericCondition> GetNumericConditions()
    {
        List<NumericCondition> result = new();
        foreach (var m in NumericConditions)
        {
            if (m is NumericCondition num)
            {
                result.Add(num);
            }
        }
        return result;
    }

    public void RemoveClassTypeCondition()
        => ClassCondition = null;

    public void RemoveRarityCondition()
        => RarityCondition = null;

    public void RemoveNumericCondition(NumericCondition condition)
        => NumericConditions.Remove(condition);

    public bool Equals(RuleModel? other) => this == other;
    public TypeConditionModel AddTypeCondition()
    {
        TypeCondition = new();
        return TypeCondition;
    }

    public bool TryGetTypeCondition(out TypeConditionModel? condition)
    {
        if (TypeCondition != null)
        {
            condition = TypeCondition;
            return true;
        }
        condition = null;
        return false;
    }

    public void RemoveTypeCondition()
    {
        TypeCondition = null;
    }

    internal void AddStateModifiers(bool mirrored, bool corrupted)
    {
        StateModifiers = new()
        {
            Mirrored = mirrored,
            Corrupted = corrupted
        };
    }

    public void RemoveStateModifiers()
    {
        StateModifiers = null;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
