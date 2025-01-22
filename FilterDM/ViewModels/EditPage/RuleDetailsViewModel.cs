using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Enums;
using FilterDM.Helpers;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;

    }

    public MapIconDecoratorViewModel AddMinimapIconModifier()
    {
        MapIconDecoratorViewModel vm = new(this, RemoveMinimapIconModifier);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public SoundDecoratorViewModel AddSoundModifier()
    {
        SoundDecoratorViewModel vm = new(this, RemoveSoundModifier);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public RarityDecoratorViewModel AddRarityFilter()
    {
        RarityDecoratorViewModel vm = new(this,RemoveRarityFilter);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddRarityFilter(RarityConditionModel model)
    {
        RarityDecoratorViewModel vm = new(this, RemoveRarityFilter);
        vm.SetModel(model);
        Messenger.Send(new FilterEditedRequestEvent(this));
        Modifiers.Add(vm);
    }

    public NumericDecoratorViewModel AddNumericFilter(NumericFilterType type)
    {
        NumericFilterHelper helper = _numericHelpers[type];
        NumericDecoratorViewModel vm = new(this, helper, RemoveNumericFilter);
        Modifiers.Add(vm);
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
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public ClassDecoratorViewModel AddClassFilter()
    {
        ClassDecoratorViewModel vm = new(this, RemoveClassFilter);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddClassFilter(ClassConditionModel condition)
    {
        ClassDecoratorViewModel vm = new(this, RemoveClassFilter);
        vm.SetModel(condition);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public TypeDecoratorViewModel AddTypeFilter()
    {
        TypeDecoratorViewModel vm = _typeScopeManager.GetDecorator(this);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddTypeFilter(TypeConditionModel condition)
    {
        TypeDecoratorViewModel vm = _typeScopeManager.GetDecorator(this);
        _typeScopeManager.SetModel(vm, condition);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public ModifierViewModelBase AddStateModifier()
    {
        ItemStateDecoratorViewModel vm = new(this, RemoveStateModifier);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    private void RemoveStateModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public void RemoveTypeFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is TypeDecoratorViewModel condition)
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveClassFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is ClassDecoratorViewModel condition)
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveNumericFilter(ModifierViewModelBase modifier)
    {
        if (modifier is NumericDecoratorViewModel condition && Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveRarityFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveSoundModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier));
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveMinimapIconModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
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
    public float CalculatedPriority => (Properties.Enabled ? -1 : 1) * Properties.Priority;

    private readonly Dictionary<NumericFilterType, NumericFilterHelper> _numericHelpers = [];
    private readonly Dictionary<string, NumericFilterHelper> _helperFromString = [];

    private readonly TypeScopeManager _typeScopeManager;
    public RuleDetailsViewModel(RuleParentManager parentManager
        , TypeScopeManager scopeManager
        , RuleTemplateManager templateManager
        , PalleteManager palleteManager)
    {
        _numericHelpers.Add(NumericFilterType.StackSize, new NumericFilterHelper(NumericFilterType.StackSize, "Stack Size", "Stack", 5000));
        _numericHelpers.Add(NumericFilterType.ItemLevel, new NumericFilterHelper(NumericFilterType.ItemLevel, "Item Level", "ILevel", 100));
        _numericHelpers.Add(NumericFilterType.DropLevel, new NumericFilterHelper(NumericFilterType.DropLevel, "Drop Level", "DLevel", 100));
        _numericHelpers.Add(NumericFilterType.AreaLevel, new NumericFilterHelper(NumericFilterType.AreaLevel, "Area Level", "ALevel", 100));
        _numericHelpers.Add(NumericFilterType.Quality, new NumericFilterHelper(NumericFilterType.Quality, "Quality", "Quality", 100));
        _numericHelpers.Add(NumericFilterType.Sockets, new NumericFilterHelper(NumericFilterType.Sockets, "Sockets Count", "Sockets", 4));
        _numericHelpers.Add(NumericFilterType.BaseArmour, new NumericFilterHelper(NumericFilterType.BaseArmour, "Base Armor", "Armor", 5000));
        _numericHelpers.Add(NumericFilterType.BaseEvasion, new NumericFilterHelper(NumericFilterType.BaseEvasion, "Base Evasion", "Evasion", 5000));
        _numericHelpers.Add(NumericFilterType.BaseEnergyShield, new NumericFilterHelper(NumericFilterType.BaseEnergyShield, "Base Energy Shield", "ES", 5000));
        _numericHelpers.Add(NumericFilterType.WaystoneTier, new NumericFilterHelper(NumericFilterType.WaystoneTier, "WaystoneTier", "T", 16));

        foreach (var value in _numericHelpers.Values)
        {
            _helperFromString[value.Name.Replace(" ", "")] = value;
            _helperFromString[value.ShortName] = value;
        }
        _typeScopeManager = scopeManager;
        Properties = new(this, parentManager, templateManager);
        Colors = new ColorDecoratorViewModel(this,palleteManager, RemoveColorModifier);
        TextSize = new TextSizeDecoratorViewModel(this, RemoveFontSizeModifier);
        Modifiers = new([Properties]);
    }

    public RuleModel GetModel()
    {
        RuleModel model = Properties.TemplateManager.GetEmpty();
        foreach (ModifierViewModelBase modifier in Modifiers)
        {
            modifier.Apply(model);
        }
        return model;
    }

    public void SetModel(RuleModel rule)
    {
        foreach(ModifierViewModelBase? modifier  in Modifiers.Skip(1).ToArray())
        {
            modifier.DeleteMeCommand.Execute(null);
        }
        Properties.SetModel(rule);
      
        if (rule.FontSize != 0 && rule.FontSize != 32)
        {
            AddFontSizeModifier().SetModel(rule);
        }

        if (rule.HasAnyColor())
        {
            AddColorsModifier().SetModel(rule);
        }
        
        if (rule.Beam != null)
        {
            AddBeamModifier().SetModel(rule.Beam);
        }

        if (rule.Icon != null)
        {
            AddMinimapIconModifier().SetModel(rule.Icon);
        }

        if (rule.Sound != null)
        {
            AddSoundModifier().SetModel(rule.Sound);
        }

        if (rule.TryGetClassCondition(out var classCondition))
        {
            AddClassFilter(classCondition);
        }

        if (rule.TryGetTypeCondition(out var typeCondition))
        {
            AddTypeFilter(typeCondition);
        }

        if (rule.TryGetRarityCondition(out var rarityCondition))
        {
            AddRarityFilter(rarityCondition);
        }


        foreach (var item in rule.GetNumericConditions())
        {
            AddNumericFilter(item);
        }

        Messenger.Send(new FilterEditedRequestEvent(this));
    }

}
