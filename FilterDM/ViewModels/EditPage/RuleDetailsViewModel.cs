﻿using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FilterDM.ViewModels.EditPage;

public partial class RuleDetailsViewModel : ObservableRecipient , IEquatable<RuleDetailsViewModel>
{
    [ObservableProperty]
    private ObservableCollection<ModifierViewModelBase> _modifiers;

    [ObservableProperty]
    private RulePropertiesDecoratorViewModel _properties;

    [ObservableProperty]
    private ColorDecoratorViewModel _colors;

    [ObservableProperty]
    private TextSizeDecoratorViewModel _textSize;

    [ObservableProperty]
    private bool _useBeam = false;

    [ObservableProperty]
    private bool _useSound = false;

    [ObservableProperty]
    private bool _useMinimapIcon = false;

    #region Filters

    [ObservableProperty]
    private bool _useRarityFilter;

    [ObservableProperty]
    private bool _useStackFilter;

    [ObservableProperty]
    private bool _useItemLevelFilter;

    [ObservableProperty]
    private bool _useDropLevelFilter;

    [ObservableProperty]
    private bool _useAreaLevelFilter;

    [ObservableProperty]
    private bool _useQualityFilter;

    [ObservableProperty]
    private bool _useSocketFilter;

    [ObservableProperty]
    private bool _useArmorFilter;

    [ObservableProperty]
    private bool _useEvasionFilter;

    [ObservableProperty]
    private bool _useESFilter;

    [ObservableProperty]
    private bool _useClassFilter;

    [ObservableProperty]
    private bool _useNameFilter;

    [ObservableProperty]
    private bool _useWaystoneFilter;

    #endregion

    [RelayCommand]
    private async Task DeleteMe()
    {
        if (Modifiers.Count > 1)
        {
            var dialogResult = await App.Current.Services.GetService<DialogService>().ShowConfirmDialog($"Are you sure to delete Rule with {Modifiers.Count} modifiers?");
            if (dialogResult)
            {
                OnDeleteConfirmed();
            }
        }
        else
        {
            OnDeleteConfirmed();
        }


    }

    public void OnDeleteConfirmed()
    {
        Messenger.Send(new DeleteRuleRequest(this));
    }


    #region Moidifiers Methods

    public TextSizeDecoratorViewModel AddFontSizeModifier()
    {
        Modifiers.Add(TextSize);
        TextSize.UseFontSize = true;
        Messenger.Send(new FilterEditedRequestEvent(this));
        return TextSize;
    }

    public ColorDecoratorViewModel AddColorsModifier()
    {
        Modifiers.Add(Colors);
        Colors.UseAnyColor = true;
        Messenger.Send(new FilterEditedRequestEvent(this));
        return Colors;
    }

    public BeamDecoratorViewModel AddBeamModifier()
    {
        BeamDecoratorViewModel vm = new(this, RemoveBeamModifier);
        UseBeam = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;

    }

    public MapIconDecoratorViewModel AddMinimapIconModifier()
    {
        MapIconDecoratorViewModel vm = new(this, RemoveMinimapIconModifier);
        UseMinimapIcon = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public SoundDecoratorViewModel AddSoundModifier()
    {
        SoundDecoratorViewModel vm = new(this, RemoveSoundModifier);
        UseSound = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public RarityDecoratorViewModel AddRarityFilter()
    {
        RarityDecoratorViewModel vm = new(this,RemoveRarityFilter);
        UseRarityFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddRarityFilter(RarityConditionModel model)
    {
        RarityDecoratorViewModel vm = new(this, RemoveRarityFilter);
        vm.SetModel(model);
        UseRarityFilter = true;
        Messenger.Send(new FilterEditedRequestEvent(this));
        Modifiers.Add(vm);
    }

    public NumericDecoratorViewModel AddNumericFilter(NumericFilterType type)
    {
        NumericFilterHelper helper = _numericHelpers[type];
        NumericDecoratorViewModel vm = new(this, helper, RemoveNumericFilter);
        Modifiers.Add(vm);
        helper.Add();
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    private void AddNumericFilter(NumericCondition condition)
    {
        string name = condition.ValueName.Replace(" ", "");

        if (_helperFromString.TryGetValue(name, out NumericFilterHelper? helper))
        {
            NumericDecoratorViewModel vm = new(this, helper, RemoveNumericFilter);
            vm.SetModel(condition);
            Modifiers.Add(vm);
            helper.Add();
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public ClassDecoratorViewModel AddClassFilter()
    {
        ClassDecoratorViewModel vm = new(this, RemoveClassFilter);
        UseClassFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddClassFilter(ClassConditionModel condition)
    {
        ClassDecoratorViewModel vm = new(this, RemoveClassFilter);
        vm.SetModel(condition);
        UseClassFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public TypeDecoratorViewModel AddTypeFilter()
    {
        TypeDecoratorViewModel vm = Properties.RealParent.GetTypeDecorator(this);
        UseNameFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddTypeFilter(TypeConditionModel condition)
    {
        TypeDecoratorViewModel vm = Properties.RealParent.GetTypeDecorator(this);
        vm.SetModel(condition);
        UseNameFilter = true;
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    public void RemoveTypeFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is TypeDecoratorViewModel condition)
        {
            UseNameFilter = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveClassFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is ClassDecoratorViewModel condition)
        {
            UseClassFilter = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveNumericFilter(ModifierViewModelBase modifier)
    {
        if (modifier is NumericDecoratorViewModel condition && Modifiers.Remove(modifier))
        {
            _numericHelpers[condition.FilterType].Remove();
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveRarityFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseRarityFilter = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveSoundModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseSound = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveMinimapIconModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseMinimapIcon = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveColorModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Colors.UseAnyColor = false;
            Colors.UseFontColor = false;
            Colors.UseBorderColor = false;
            Colors.UseBackColor = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }

    }

    private void RemoveBeamModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            UseBeam = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveFontSizeModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            TextSize.UseFontSize = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public bool Equals(RuleDetailsViewModel? other)
    {
        return other == this;
    }

    #endregion
    public float CalculatedPriority => Properties.Enabled ? -1 : 1 * Properties.Priority;

    private readonly Dictionary<NumericFilterType, NumericFilterHelper> _numericHelpers = [];
    private readonly Dictionary<string, NumericFilterHelper> _helperFromString = [];

    public RuleDetailsViewModel(ObservableCollection<BlockDetailsViewModel> allBlocks
        , BlockDetailsViewModel parentBlock
        , ObservableCollection<string> templates)
    {
        _numericHelpers.Add(NumericFilterType.StackSize, new NumericFilterHelper(NumericFilterType.StackSize, "Stack Size", "Stack", 5000, (x) => UseStackFilter = x));
        _numericHelpers.Add(NumericFilterType.ItemLevel, new NumericFilterHelper(NumericFilterType.ItemLevel, "Item Level", "ILevel", 100, (x) => UseItemLevelFilter = x));
        _numericHelpers.Add(NumericFilterType.DropLevel, new NumericFilterHelper(NumericFilterType.DropLevel, "Drop Level", "DLevel", 100, (x) => UseDropLevelFilter = x));
        _numericHelpers.Add(NumericFilterType.AreaLevel, new NumericFilterHelper(NumericFilterType.AreaLevel, "Area Level", "ALevel", 100, (x) => UseAreaLevelFilter = x));
        _numericHelpers.Add(NumericFilterType.Quality, new NumericFilterHelper(NumericFilterType.Quality, "Quality", "Quality", 100, (x) => UseQualityFilter = x));
        _numericHelpers.Add(NumericFilterType.Sockets, new NumericFilterHelper(NumericFilterType.Sockets, "Sockets Count", "Sockets", 4, (x) => UseSocketFilter = x));
        _numericHelpers.Add(NumericFilterType.BaseArmour, new NumericFilterHelper(NumericFilterType.BaseArmour, "Base Armor", "Armor", 5000, (x) => UseArmorFilter = x));
        _numericHelpers.Add(NumericFilterType.BaseEvasion, new NumericFilterHelper(NumericFilterType.BaseEvasion, "Base Evasion", "Evasion", 5000, (x) => UseEvasionFilter = x));
        _numericHelpers.Add(NumericFilterType.BaseEnergyShield, new NumericFilterHelper(NumericFilterType.BaseEnergyShield, "Base Energy Shield", "ES", 5000, (x) => UseESFilter = x));
        _numericHelpers.Add(NumericFilterType.WaystoneTier, new NumericFilterHelper(NumericFilterType.WaystoneTier, "WaystoneTier", "T", 16, (x) => UseWaystoneFilter = x));

        foreach (var value in _numericHelpers.Values)
        {
            _helperFromString[value.Name.Replace(" ", "")] = value;
            _helperFromString[value.ShortName] = value;
        }

        Properties = new(this, allBlocks, parentBlock, templates);
        Colors = new ColorDecoratorViewModel(this, RemoveColorModifier);
        TextSize = new TextSizeDecoratorViewModel(this, RemoveFontSizeModifier);
    }

    public RuleModel GetModel()
    {
        RuleModel model = App.Current.Services.GetService<RuleTemplateService>().BuildEmpty();
        foreach (ModifierViewModelBase modifier in Modifiers)
        {
            modifier.Apply(model);
        }
        return model;
    }

    public void SetModel(RuleModel rule)
    {
        Modifiers = [Properties];
        Properties.SetModel(rule);
      
        if (rule.FontSize != 0 && rule.FontSize != 32)
        {
            AddFontSizeModifier().SetModel(rule);
        }
        else
        {
            TextSize.UseFontSize = false;
        }
        if (rule.HasAnyColor())
        {
            AddColorsModifier().SetModel(rule);
        }
        else
        {
            Colors.UseAnyColor = false;
            Colors.UseBackColor = false;
            Colors.UseBorderColor = false;
            Colors.UseFontColor = false;
        }
        
        if (rule.Beam != null)
        {
            AddBeamModifier().SetModel(rule.Beam);
        }
        else
        {
            UseBeam = false;
        }


        if (rule.Icon != null)
        {
            AddMinimapIconModifier().SetModel(rule.Icon);
        }
        else
        {
            UseMinimapIcon = false;
        }

        if (rule.Sound != null)
        {
            AddSoundModifier().SetModel(rule.Sound);
        }
        else
        {
            UseSound = false;
        }

        if (rule.TryGetClassCondition(out var classCondition))
        {
            AddClassFilter(classCondition);
        }
        else
        {
            UseClassFilter = false;
        }

        if (rule.TryGetTypeCondition(out var typeCondition))
        {
            AddTypeFilter(typeCondition);
        }
        else
        {
            UseNameFilter = false;
        }


        if (rule.TryGetRarityCondition(out var rarityCondition))
        {
            AddRarityFilter(rarityCondition);
        }
        else
        {
            UseRarityFilter = false;
        }

        foreach (var helper in _numericHelpers.Values)
        {
            helper.Remove();
        }

        foreach (var item in rule.GetNumericConditions())
        {
            AddNumericFilter(item);
        }

        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    internal void DeleteSafe()
    {
        Messenger.Send(new DeleteRuleRequest(this));
    }
}
