using FilterDM.Enums;
using FilterDM.Helpers;
using FilterDM.Managers;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using System;

namespace FilterDM.Factories;
public class ModifiersFactory : IModifiersFactory
{
    private readonly MinimapIconsService _iconService;
    private readonly SoundService _soundService;

    public ModifiersFactory(MinimapIconsService iconService, SoundService soundService)
    {
        _iconService = iconService;
        _soundService = soundService;
    }

    public ColorDecoratorViewModel BuildColorDecorator(RuleDetailsViewModel ruleDetailsViewModel
        , PalleteManager palleteManager
        , Action<ModifierViewModelBase> removeColorModifier)
    {
        ColorDecoratorViewModel vm = new(palleteManager)
        {
            Rule = ruleDetailsViewModel,
            DeleteAction = removeColorModifier,
        };
        return vm;

    }
    public T BuildDecorator<T>(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> deleteCallback) where T : ModifierViewModelBase, new()
    {
        T decorator = new()
        {
            Rule = ruleDetailsViewModel,
            DeleteAction = deleteCallback
        };
        return decorator;
    }
    public MapIconDecoratorViewModel BuildMinimapIconDecorator(RuleDetailsViewModel ruleDetailsViewModel
        , Action<ModifierViewModelBase> removeMinimapIconModifier)
    {
        MapIconDecoratorViewModel vm = new(_iconService)
        {
            Rule = ruleDetailsViewModel,
            DeleteAction = removeMinimapIconModifier,
        };
        return vm;
    }
    public NumericDecoratorViewModel BuildNumericDecorator(RuleDetailsViewModel ruleDetailsViewModel
        , NumericFilterType numericType)
    {
        var helper = BuildHelper(ruleDetailsViewModel, numericType);
        NumericDecoratorViewModel vm = new(helper)
        {
            Rule = ruleDetailsViewModel,
            DeleteAction = ruleDetailsViewModel.RemoveNumericFilter
        };
        return vm;
    }

    private static NumericFilterHelper BuildHelper(RuleDetailsViewModel ruleDetailsViewModel, NumericFilterType type)
    {
        switch (type)
        {
            case NumericFilterType.StackSize:
            return new NumericFilterHelper(NumericFilterType.StackSize, "Stack Size", "Stack", 5000);
            case NumericFilterType.ItemLevel:
            return new NumericFilterHelper(NumericFilterType.ItemLevel, "Item Level", "ILevel", 100);
            case NumericFilterType.DropLevel:
            return new NumericFilterHelper(NumericFilterType.DropLevel, "Drop Level", "DLevel", 100);
            case NumericFilterType.AreaLevel:
            return new NumericFilterHelper(NumericFilterType.AreaLevel, "Area Level", "ALevel", 100);
            case NumericFilterType.Quality:
            return new NumericFilterHelper(NumericFilterType.Quality, "Quality", "Quality", 100);
            case NumericFilterType.Sockets:
            return new NumericFilterHelper(NumericFilterType.Sockets, "Sockets Count", "Sockets", 4);
            case NumericFilterType.BaseArmour:
            return new NumericFilterHelper(NumericFilterType.BaseArmour, "Base Armor", "Armor", 5000);
            case NumericFilterType.BaseEvasion:
            return new NumericFilterHelper(NumericFilterType.BaseEvasion, "Base Evasion", "Evasion", 5000);
            case NumericFilterType.BaseEnergyShield:
            return new NumericFilterHelper(NumericFilterType.BaseEnergyShield, "Base Energy Shield", "ES", 5000);
            case NumericFilterType.WaystoneTier:
            return new NumericFilterHelper(NumericFilterType.WaystoneTier, "WaystoneTier", "T", 16);
            case NumericFilterType.Width:
            return new NumericFilterHelper(NumericFilterType.Width, "Item Width", "w", 2);
            case NumericFilterType.Height:
            return new NumericFilterHelper(NumericFilterType.Width, "Item Height", "w", 3);
        }
        return null;
    }

    public SoundDecoratorViewModel BuildSoundDecorator(RuleDetailsViewModel ruleDetailsViewModel
        , Action<ModifierViewModelBase> removeSoundModifier)
    {
        SoundDecoratorViewModel vm = new(_soundService)
        {
            Rule = ruleDetailsViewModel,
            DeleteAction = removeSoundModifier,
        };
        return vm;
    }
    public TypeDecoratorViewModel BuildTypeDecorator(RuleDetailsViewModel ruleDetailsViewModel
        , TypeScopeManager typeScopeManager)
    {
        return typeScopeManager.GetDecorator(ruleDetailsViewModel);
    }
}
