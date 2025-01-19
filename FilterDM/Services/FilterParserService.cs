using Avalonia.Media;
using FilterCore.Parser;
using FilterDM.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterDM.Services;

public struct ImportResult
{
    public readonly string ErorrMessage;
    public readonly ParseReuslt ParseResult;
    public FilterModel Model { get; set;  }

    public ImportResult(string erorrMessage, ParseReuslt parseResult)
    {
        ErorrMessage = erorrMessage;
        ParseResult = parseResult;
    }
}

public class FilterParserService
{
    
    public ImportResult Parse(string input)
    {
        FilterModel model = new FilterModel();

        ImportResult result = ParseRules(input);

        List<RuleModel> models = [];
        if (result.ParseResult.Result.Count > 0)
        {
            int i = 2000 +  result.ParseResult.Result.Count * 100;
            foreach (var rule in result.ParseResult.Result)
            {
                try
                {
                    RuleModel rm = ParseSingleRule(rule, i);
                    models.Add(rm);
                }
                catch (Exception ex)
                {
                    int index = result.ParseResult.Result.IndexOf(rule);
                }
              
                i -= 100;
            }

            Dictionary<string, List<RuleModel>> blockData = [];
            blockData.Add("unknown", []);
            foreach (RuleModel pModel in models)
            {
                string nameIdentifier = pModel.Title.Split('|').First();
                if (nameIdentifier != null && nameIdentifier.Length > 1)
                {
                    List<RuleModel> selectedList = blockData.GetValueOrDefault(nameIdentifier, []);
                    selectedList.Add(pModel);
                    blockData[nameIdentifier] = selectedList;
                }
                else
                {
                    blockData["unknown"].Add(pModel);
                }
            }
            if (blockData["unknown"].Count == 0)
            {
                blockData.Remove("unknown");
            }

            foreach (string key in blockData.Keys)
            {
                BlockModel newBlock = App.Current.Services.GetService<BlockTemplateRepository>().GetEmpty();
                newBlock.Title = key;
                foreach (var m in blockData[key])
                {
                    newBlock.AddRule(m);
                    newBlock.Priority += m.Priority;
                }
                model.AddBlock(newBlock);
            }

            result.Model = model;

        }

        return result;
    }

    private ImportResult ParseRules(string input)
    {
        var lexer = new FilterLexer();
        List<Token> tokens;
        try
        {
            tokens = lexer.BuildTokens(input);

        }
        catch (LexerError e)
        {
            return new ImportResult(e.Message, new ParseReuslt(1, []));
        }
        var parser = new RuleParser();
        List<Rule> rules = parser.Parse(tokens);
        ParseReuslt res = new ParseReuslt(parser.Errors.Count, rules);
        return new ImportResult(parser.Errors.Count > 0 ? parser.Errors[0] : "", res);
    }

    private RuleModel ParseSingleRule(Rule rule, int priority)
    {
        RuleModel model = App.Current.Services.GetService<RuleTemplateService>().BuildEmpty();
        model.Show = rule.StartToken.Value.Equals("Show") ? true : false;
        model.Priority = priority;


        foreach (var node in rule.Nodes)
        {
            switch (node.Operator.type)
            {
                case TokenType.CLASS_DECORATOR:
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
                case TokenType.TYPE_DECORATOR:
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

                case TokenType.NUMERIC_DECORATOR:
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
                    condition.Number = number;


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
                    model.AddNumericCondition(condition);

                }
                break;

                case TokenType.COLOR_DECORATOR:
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
                case TokenType.TEXT_SIZE_DECORATOR:
                {
                    int size = Math.Clamp(int.Parse(node.Parameters[0].Value), 12, 45);
                    model.FontSize = size;
                }
                break;

                case TokenType.BEAM_DECORATOR:
                {
                    BeamDetails beam = new BeamDetails();
                    beam.Color = node.Parameters[0].Value;
                    beam.IsPermanent = node.Parameters.Count == 1;

                    model.Beam = beam;

                }
                break;
                case TokenType.MINIMAP_DECORATOR:
                {
                    MinimapIconDetails icon = new MinimapIconDetails();
                    int size  = int.Parse(node.Parameters[0].Value);
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

                case TokenType.SOUND_DECORATOR:
                {
                    SoundDetails sound = new SoundDetails();
                    sound.Sample = int.Parse(node.Parameters[0].Value);
                    sound.Volume = int.Parse(node.Parameters[1].Value);
                    model.Sound = sound;
                }
                break;

             

                case TokenType.SINGLE_DECORATOR:
                break;

                case TokenType.RARITY_DECORATOR:
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
        if (model.TypeCondition != null )
        {
            if (model.TypeCondition.SelectedTypes.Count >4)
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
            NumericCondition n= model.GetNumericConditions()[0];

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
        model.Title = title ;
        if (rule.Nodes.Select(x=> x.Operator.type == TokenType.CONTINUE).First())
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

public readonly struct ParseReuslt
{
    public readonly int ErrCount;
    public readonly List<Rule> Result;

    public ParseReuslt(int errCount, List<Rule> result)
    {
        ErrCount = errCount;
        Result = result;
    }
}