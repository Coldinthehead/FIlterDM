using Avalonia.Media;
using FilterCore.Parser;
using FilterDM.Models;
using FilterDM.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterDM.Services;

public struct ImportResult
{
    public List<string> Errors { get; set; }
    public FilterModel Model { get; set;  }

    public int TotalRules;

    public ImportResult()
    {
        Errors = [];
    }
}

public struct ParseResult
{
    public readonly List<string> Errors;
    public readonly List<Rule> Rules;

    public ParseResult()
    {
        Errors = [];
        Rules = [];
    }
}

public class FilterParserService
{
    public ImportResult Parse(string input)
    {
        FilterModel model = new FilterModel();

        ParseResult parseResult = ParseRules(input);
        ImportResult importResult = new();
        importResult.Errors.AddRange(parseResult.Errors);
        List<RuleModel> models = [];

        if (parseResult.Rules.Count > 0)
        {
            int priority = 2000 * parseResult.Rules.Count;
            foreach (Rule rule in parseResult.Rules)
            {
                try
                {
                    RuleModel ruleModel = ParseSingleRule(rule, priority);
                    models.Add(ruleModel);
                }
                catch (Exception e)
                {
                    importResult.Errors.Add(e.Message);
                }
                priority -= 2000;
            }
        }
        importResult.TotalRules = parseResult.Rules.Count;
        BlockModel rootBlock = model.AddBlock("rules");
        foreach (var r in models)
        {
            rootBlock.AddRule(r);
        }
        importResult.Model = model;

        return importResult;
    }

    private ParseResult ParseRules(string input)
    {
        ParseResult result = new();
        var lexer = new FilterLexer();
        List<Token> tokens;
        try
        {
            tokens = lexer.BuildTokens(input);

        }
        catch (LexerError e)
        {
            result.Errors.Add(e.Message);
            return result;
        }
        RuleParser parser = new RuleParser();
        List<Rule> rules = parser.Parse(tokens);
        result.Errors.AddRange(parser.Errors);
        TypeResolver argTypeResolver = new TypeResolver();
        ModifierResolver modResolver = new();
        foreach (Rule rule in rules)
        {
            foreach (RuleNode node in rule.Nodes)
            {
                argTypeResolver.Resolve(node);
                modResolver.Resolve(node);
            }
        }

        result.Rules.AddRange(rules);

        return result;
    }

    private RuleModel ParseSingleRule(Rule rule, int priority)
    {
        RuleModel model = App.Current.Services.GetService<RuleTemplateRepository>().GetEmpty();
        model.Show = rule.StartToken.Value.Equals("Show") ? true : false;
        model.Priority = priority;


        foreach (var node in rule.Nodes)
        {
            switch (node.GetOperatorMetaType())
            {
                case ModifierType.Class:
                {
                    ClassConditionModel condition = new();
                    var classService = App.Current.Services.GetService<ItemClassesService>();

                    if (node.Parameters[0].Value.Equals("="))
                    {
                        List<string> patterns = node.Parameters.Slice(1, node.Parameters.Count - 1).Select(x => x.Value).ToList();
                        List<string> resultValues = [];
                        foreach (string p in patterns)
                        {
                            resultValues.AddRange(classService.GetPartialMatches(p));
                        }
                        condition.AddRange(resultValues);
                    }
                    else if (node.Parameters[0].Value.Equals("=="))
                    {
                        List<string> patterns = node.Parameters.Slice(1, node.Parameters.Count - 1).Select(x => x.Value).ToList();
                        List<string> resultValues = [];
                        foreach (string p in patterns)
                        {
                            resultValues.AddRange(classService.GetExactMatches(p));
                        }
                        condition.AddRange(resultValues);
                    }
                    else
                    {
                        List<string> patterns = node.Parameters.Select(x => x.Value).ToList();
                        List<string> resultValues = [];
                        foreach (string p in patterns)
                        {
                            resultValues.AddRange(classService.GetPartialMatches(p));
                        }
                        condition.AddRange(resultValues);
                    }

                    model.ClassCondition = condition;

                }
                break;
                case ModifierType.BaseType:
                {
                    TypeConditionModel condition = new();

                    var typeService = App.Current.Services.GetService<ItemTypeService>();

                    if (node.Parameters[0].Value.Equals("="))
                    {
                        List<string> patterns = node.Parameters.Slice(1, node.Parameters.Count - 1).Select(x => x.Value).ToList();
                        List<string> resultValues = [];
                        foreach (var p in patterns)
                        {
                            resultValues.AddRange(typeService.GetPartialMatches(p));
                        }
                        condition.AddRange(resultValues);
                    }
                    else if (node.Parameters[0].Value.Equals("=="))
                    {
                        List<string> patterns = node.Parameters.Slice(1, node.Parameters.Count - 1).Select(x => x.Value).ToList();
                        List<string> resultValues = [];
                        foreach (var p in patterns)
                        {
                            resultValues.AddRange(typeService.GetExactMatches(p));
                        }
                        condition.AddRange(resultValues);
                    }
                    else
                    {
                        List<string> patterns = node.Parameters.Select(x => x.Value).ToList();
                        List<string> resultValues = [];
                        foreach (var p in patterns)
                        {
                            resultValues.AddRange(typeService.GetPartialMatches(p));
                        }
                        condition.AddRange(resultValues);
                    }


                    model.TypeCondition = condition;
                }
                break;

                case ModifierType.AreaLevel:
                case ModifierType.DropLevel:
                case ModifierType.Quality:
                case ModifierType.ItemLevel:
                case ModifierType.Height:
                case ModifierType.Width:
                case ModifierType.WaystoneTier:
                case ModifierType.BaseAmrmour:
                case ModifierType.BaseEvasion:
                case ModifierType.BaseEnergyShield:
                case ModifierType.Sockets:
                {
                    NumericCondition condition = new();
                    string name = node.Operator.Value;
                    string sign = "";
                    int number;
                    if (node.Parameters.Count == 1)
                    {
                        sign = "=";
                        number = int.Parse(node.Parameters[0].Value);
                    }
                    else
                    {
                        sign = node.Parameters[0].Value;
                        number = int.Parse(node.Parameters[1].Value);
                    }

                    condition.ValueName = name;

                    if (sign.Equals("==") || sign == "=")
                    {
                        condition.UseEquals = NumericConditionSign.Eq;
                    }
                    else if (sign.Equals(">="))
                    {
                        condition.UseEquals = NumericConditionSign.Greater;
                    }
                    else if (sign.Equals(">"))
                    {
                        condition.UseEquals = NumericConditionSign.Greater;
                        number++;
                    }
                    else if (sign.Equals("<"))
                    {
                        condition.UseEquals = NumericConditionSign.Less;
                    }
                    else if (sign.Equals("<="))
                    {
                        condition.UseEquals = NumericConditionSign.Less;
                        number++;
                    }
                    else
                    {
                        break;
                    }
                    condition.Number = number;

                    model.AddNumericCondition(condition);

                }
                break;

                case ModifierType.SetBackgroundColor:
                case ModifierType.SetBorderColor:
                case ModifierType.SetTextColor:
                {
                    var value = node.Operator.Value;
                    if (value.Equals("SetBorderColor"))
                    {
                        int R = int.Parse(node.Parameters[0].Value);
                        int G = int.Parse(node.Parameters[1].Value);
                        int B = int.Parse(node.Parameters[2].Value);
                        int A = node.Parameters.Count > 3 ? int.Parse(node.Parameters[3].Value) : 255;
                        var c = Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
                        model.AddBorderColor(c);
                    }
                    else if (value.Equals("SetTextColor"))
                    {
                        int R = int.Parse(node.Parameters[0].Value);
                        int G = int.Parse(node.Parameters[1].Value);
                        int B = int.Parse(node.Parameters[2].Value);
                        int A = node.Parameters.Count > 3 ? int.Parse(node.Parameters[3].Value) : 255;
                        var c = Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
                        model.AddTextColor(c);

                    }
                    else if (value.Equals("SetBackgroundColor"))
                    {
                        int R = int.Parse(node.Parameters[0].Value);
                        int G = int.Parse(node.Parameters[1].Value);
                        int B = int.Parse(node.Parameters[2].Value);
                        int A = node.Parameters.Count > 3 ? int.Parse(node.Parameters[3].Value) : 255;
                        var c = Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
                        model.AddBackgroundColor(c);
                    }
                }
                break;
                case ModifierType.SetFontSize:
                {
                    int size = Math.Clamp(int.Parse(node.Parameters[0].Value), 12, 45);
                    model.FontSize = size;
                }
                break;

                case ModifierType.PlayEffect:
                {
                    BeamDetails beam = new BeamDetails();
                    beam.Color = node.Parameters[0].Value;
                    beam.IsPermanent = node.Parameters.Count == 1;

                    model.Beam = beam;

                }
                break;
                case ModifierType.MinimapIcon:
                {
                    MinimapIconDetails icon = new MinimapIconDetails();
                    int size = int.Parse(node.Parameters[0].Value);
                    if (size == 0)
                    {
                        icon.Size = "Small";
                    }
                    else if (size == 1)
                    {
                        icon.Size = "Medium";
                    }
                    else if (size == 2)
                    {
                        icon.Size = "Large";
                    }
                    else
                    {
                        break;
                    }
                    icon.Color = node.Parameters[1].Value;
                    icon.Shape = node.Parameters[2].Value;

                    model.Icon = icon;
                }
                break;

                case ModifierType.PlayAlertSound:
                case ModifierType.PlayAlertSoundPositional:
                {
                    SoundDetails sound = new SoundDetails();
                    sound.Sample = int.Parse(node.Parameters[0].Value);
                    sound.Volume = int.Parse(node.Parameters[1].Value);
                    model.Sound = sound;
                }
                break;



                case ModifierType.DisableDropSound:
                case ModifierType.EnableDropSound:
                break;

                case ModifierType.Rarity:
                {
                    RarityConditionModel condition = new();
                    List<string> rarityMap = ["Normal", "Magic", "Rare", "Unique"];

                    if (node.Parameters[0].type == TokenType.BOOL_OPERATOR)
                    {
                        var sign = node.Parameters[0].Value;
                        if (sign.Equals("=="))
                        {
                            condition.UseRarity(node.Parameters[1].Value);
                        }
                        else if (sign.Equals("!="))
                        {
                            foreach (Token t in node.Parameters)
                            {
                                condition.UseRarity(t.Value);
                            }
                        }
                        else if (sign.Equals(">"))
                        {
                            int index = rarityMap.IndexOf(node.Parameters[1].Value);
                            for (int i = index + 1; i < rarityMap.Count; i++)
                            {
                                condition.UseRarity(rarityMap[i]);
                            }
                        }
                        else if (sign.Equals(">="))
                        {
                            int index = rarityMap.IndexOf(node.Parameters[1].Value);
                            for (int i = index; i < rarityMap.Count; i++)
                            {
                                condition.UseRarity(rarityMap[i]);
                            }
                        }
                        else if (sign.Equals("<"))
                        {
                            int index = rarityMap.IndexOf(node.Parameters[1].Value);
                            for (int i = index - 1; i >= 0; i--)
                            {
                                condition.UseRarity(rarityMap[i]);
                            }
                        }
                        else if (sign.Equals("<="))
                        {
                            int index = rarityMap.IndexOf(node.Parameters[1].Value);
                            for (int i = index; i >= 0; i--)
                            {
                                condition.UseRarity(rarityMap[i]);
                            }
                        }
                    }
                    else
                    {
                        foreach (var t in node.Parameters)
                        {
                            condition.UseRarity(t.Value);
                        }
                    }


                    model.RarityCondition = condition;
                }
                break;
            }
        }

        string title = "";
        if (model.ClassCondition != null && model.ClassCondition.SelectedClasses.Count > 0)
        {
            title += $"{model.ClassCondition.SelectedClasses[0]}|";
        }
        if (model.TypeCondition != null)
        {
            if (model.TypeCondition.SelectedTypes.Count > 4)
            {
                title += $"{model.TypeCondition.SelectedTypes.Count} types|";
            }
            else if (model.TypeCondition.SelectedTypes.Count >= 1)
            {
                title += $"{model.TypeCondition.SelectedTypes[0]}|";
            }
        }
        if (model.RarityCondition != null)
        {
            title += $"{model.RarityCondition.GetTitle()}|";
        }
        if (model.GetNumericConditions().Count > 0)
        {
            NumericCondition n = model.GetNumericConditions()[0];

            string sign;
            if (n.UseEquals == NumericConditionSign.Less)
            {
                sign = $"-{n.Number}";
            }
            else if (n.UseEquals == NumericConditionSign.Greater)
            {
                sign = $"{n.Number}+";
            }
            else
            {
                sign = $"{n.Number}";
            }

            title += $"{n.ValueName} {sign}|";
        }
        model.Title = title;
        if (rule.Nodes.Select(x => x.Operator.type == TokenType.CONTINUE).First())
        {
            model.Title = "decorator";
        }

        if (model.Title == "")
        {
            model.Title = "unknown";
        }

        return model;
    }
}