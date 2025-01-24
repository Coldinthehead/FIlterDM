using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Enums;
using FilterDM.Factories;
using FilterDM.Helpers;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.ViewModels.EditPage.Events;
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

    [ObservableProperty]
    private MapIconDecoratorViewModel _mapIcon;

    [RelayCommand]
    private async Task DeleteMe()
    {
        if (Modifiers.Count > 1)
        {
            var dialogResult = await _dialogService.ShowConfirmDialog($"Are you sure to delete Rule with {Modifiers.Count} modifiers?");
            if (!dialogResult)
            {
                return;
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
        BeamDecoratorViewModel vm = _modifiersFactory.BuildDecorator<BeamDecoratorViewModel>(this, RemoveBeamModifier);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public MapIconDecoratorViewModel AddMinimapIconModifier()
    {
        MapIcon.UseMinimapIcon = true;
        Modifiers.Add(MapIcon);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return MapIcon;
    }

    public SoundDecoratorViewModel AddSoundModifier()
    {
        SoundDecoratorViewModel vm = _modifiersFactory.BuildSoundDecorator(this, RemoveSoundModifier);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public RarityDecoratorViewModel AddRarityFilter()
    {
        RarityDecoratorViewModel vm = _modifiersFactory.BuildDecorator<RarityDecoratorViewModel>(this, RemoveRarityFilter);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddRarityFilter(RarityConditionModel model)
    {
        RarityDecoratorViewModel vm = _modifiersFactory.BuildDecorator<RarityDecoratorViewModel>(this, RemoveRarityFilter);
        vm.SetModel(model);
        Messenger.Send(new FilterEditedRequestEvent(this));
        Modifiers.Add(vm);
    }

    public NumericDecoratorViewModel AddNumericFilter(NumericFilterType type)
    {
        NumericDecoratorViewModel vm = _modifiersFactory.BuildNumericDecorator(this, type);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddNumericFilter(NumericCondition condition)
    {
        string name = condition.ValueName.Replace(" ", "");
        if (Enum.TryParse(typeof(NumericFilterType), name, out object? result))
        {
            var numericType = (NumericFilterType)result;
            NumericDecoratorViewModel vm = _modifiersFactory.BuildNumericDecorator(this, numericType);
            vm.SetModel(condition);
            Modifiers.Add(vm);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public ClassDecoratorViewModel AddClassFilter()
    {
        ClassDecoratorViewModel vm = _modifiersFactory.BuildDecorator<ClassDecoratorViewModel>(this, RemoveClassFilter);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    public void AddClassFilter(ClassConditionModel condition)
    {
        ClassDecoratorViewModel vm = _modifiersFactory.BuildDecorator<ClassDecoratorViewModel>(this, RemoveClassFilter);
        vm.SetModel(condition);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
    }


    public TypeDecoratorViewModel AddTypeFilter()
    {
        TypeDecoratorViewModel vm = _modifiersFactory.BuildTypeDecorator(this, _typeScopeManager);
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


    public ItemStateDecoratorViewModel AddStateModifier()
    {
        ItemStateDecoratorViewModel vm = _modifiersFactory.BuildDecorator<ItemStateDecoratorViewModel>(this, RemoveStateModifier);
        Modifiers.Add(vm);
        Messenger.Send(new FilterEditedRequestEvent(this));
        return vm;
    }

    private void RemoveStateModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public void RemoveTypeFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is TypeDecoratorViewModel condition)
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveClassFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier) && modifier is ClassDecoratorViewModel condition)
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public void RemoveNumericFilter(ModifierViewModelBase modifier)
    {
        if (modifier is NumericDecoratorViewModel condition && Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveRarityFilter(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveSoundModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveMinimapIconModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            MapIcon.UseMinimapIcon = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
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
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }

    }

    private void RemoveBeamModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    private void RemoveFontSizeModifier(ModifierViewModelBase modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            TextSize.UseFontSize = false;
            Messenger.Send(new RuleModifierDeleteEvent(modifier), this);
            Messenger.Send(new FilterEditedRequestEvent(this));
        }
    }

    public bool Equals(RuleDetailsViewModel? other)
    {
        return other == this;
    }

    #endregion
    public float CalculatedPriority => (Properties.Enabled ? -1 : 1) * Properties.Priority;

    private readonly TypeScopeManager _typeScopeManager;
    private readonly DialogService _dialogService;
    private readonly IModifiersFacotry _modifiersFactory;
    public RuleDetailsViewModel(
        RuleParentManager parentManager
        , TypeScopeManager scopeManager
        , RuleTemplateManager templateManager
        , PalleteManager palleteManager
        , DialogService dialogService
        , IModifiersFacotry modifiersFactory)
    {

        _typeScopeManager = scopeManager;
        Properties = new(this, parentManager, templateManager, dialogService);
        Colors = modifiersFactory.BuildColorDecorator(this, palleteManager, RemoveColorModifier);
        TextSize = modifiersFactory.BuildDecorator<TextSizeDecoratorViewModel>(this, RemoveFontSizeModifier);
        Modifiers = new([Properties]);
        MapIcon = modifiersFactory.BuildMinimapIconDecorator(this, RemoveMinimapIconModifier);
        _dialogService = dialogService;
        _modifiersFactory = modifiersFactory;
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

        if (rule.StateModifiers != null)
        {
            AddStateModifier().SetModel(rule.StateModifiers);
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
