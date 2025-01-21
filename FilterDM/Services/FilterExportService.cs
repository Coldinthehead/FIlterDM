using FilterDM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilterDM.Services;

public class FilterExportService
{

    public string Build(FilterModel model)
    {
        var sb = new StringBuilder();
        foreach (var block in model.Blocks.Where(x => x.Enabled).OrderByDescending(x => x.Priority))
        {
            sb.AppendLine($"#@{block.Title}");
            sb.AppendLine(BuildBlock(block));
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private string BuildBlock(BlockModel block)
    {
        var sb = new StringBuilder();

        foreach (var rule in block.Rules.Where(x => x.Enabled).OrderByDescending(x => x.Priority))
        {
            sb.AppendLine($"#${rule.Title}");
            sb.AppendLine(BuildRule(rule));
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private string BuildRule(RuleModel rule)
    {
        var sb = new StringBuilder();

        if (rule.Show)
        {
            sb.AppendLine("Show");
        }
        else
        {
            sb.AppendLine("Hide");
        }

        if (rule.ClassCondition != null && rule.ClassCondition.SelectedClasses.Count > 0)
        {
            sb.AppendLine($"\tClass {string.Join(" ", rule.ClassCondition.SelectedClasses.Select(x => $"\"{x}\"").ToList())}");
        }
        if (rule.TypeCondition != null && rule.TypeCondition.SelectedTypes.Count > 0)
        {
            sb.AppendLine($"\tBaseType {string.Join(" ", rule.TypeCondition.SelectedTypes.Select(x => $"\"{x}\"").ToList())}");
        }
        if (rule.RarityCondition != null)
        {
            List<string> rarity = [];
            if (rule.RarityCondition.UseNormal)
            {
                rarity.Add("Normal");
            }
            if (rule.RarityCondition.UseMagic)
            {
                rarity.Add("Magic");
            }
            if (rule.RarityCondition.UseRare)
            {
                rarity.Add("Rare");
            }
            if (rule.RarityCondition.UseUnique)
            {
                rarity.Add("Unique");
            }
            if (rarity.Count != 0)
            {
                sb.AppendLine($"\tRarity {string.Join(" ", rarity.Select(x => $"\"{x}\"").ToList())}");

            }
        }
        foreach (var numeric in rule.NumericConditions)
        {
            sb.AppendLine(BuildNumericCondition(numeric));
        }
        if (rule.FontSize != 0 && rule.FontSize != 32)
        {
            sb.AppendLine($"\tSetFontSize {rule.FontSize}");
        }
        if (rule.TryGetTextColor(out var textColor))
        {
            sb.AppendLine($"\tSetTextColor {textColor.R} {textColor.G} {textColor.B} {textColor.A}");
        }
        if (rule.TryGetBackgroundColor(out var bGcolor))
        {
            sb.AppendLine($"\tSetBackgroundColor {bGcolor.R} {bGcolor.G} {bGcolor.B} {bGcolor.A}");
        }
        if (rule.TryGetBorderColor(out var borderColor))
        {
            sb.AppendLine($"\tSetBorderColor {borderColor.R} {borderColor.G} {borderColor.B} {borderColor.A}");
        }
        if (rule.Beam != null)
        {
            var tempStr = rule.Beam.IsPermanent ? "" : "Temp";
            sb.AppendLine($"\tPlayEffect {rule.Beam.Color} {tempStr}");
        }
        if (rule.Icon != null)
        {
            var size = rule.Icon.Size.ToLower();
            int s = 0;
            if (size.Equals("small"))
            {
                s = 0;
            }
            else if (size.Equals("medium"))
            {
                s = 1;
            }
            else if (size.Equals("large"))
            {
                s = 2;
            }
            sb.AppendLine($"\tMinimapIcon {s} {rule.Icon.Color} {rule.Icon.Shape}");
        }
        if (rule.Sound != null)
        {
            sb.AppendLine($"\tPlayAlertSound {rule.Sound.Sample} {rule.Sound.Volume}");
        }
        return sb.ToString();
    }

    private readonly Dictionary<NumericConditionSign, string> _operatorTypeToString = new()
    {
        { NumericConditionSign.Less, "<" },
        { NumericConditionSign.Eq, "==" },
        { NumericConditionSign.Greater, ">=" }
    };

    private string BuildNumericCondition(NumericCondition condition)
    {
        var name = condition.ValueName;
        var sign = _operatorTypeToString[condition.UseEquals];
        return $"\t{name} {sign} {condition.Number}";
    }

}
