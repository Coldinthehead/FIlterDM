using FilterDM.Enums;
using FilterDM.Managers;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using System;

namespace FilterDM.Factories;

public interface IModifiersFacotry
{
    ColorDecoratorViewModel BuildColorDecorator(RuleDetailsViewModel ruleDetailsViewModel, PalleteManager palleteManager, Action<ModifierViewModelBase> removeColorModifier);
    T BuildDecorator<T>(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> deleteCallback) where T : ModifierViewModelBase;
    MapIconDecoratorViewModel BuildMinimapIconDecorator(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> removeMinimapIconModifier);
    NumericDecoratorViewModel BuildNumericDecorator(RuleDetailsViewModel ruleDetailsViewModel, NumericFilterType numericType);
    TypeDecoratorViewModel BuildTypeDecorator(RuleDetailsViewModel ruleDetailsViewModel, TypeScopeManager typeScopeManager);
}
