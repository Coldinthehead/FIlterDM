using FilterDM.Enums;
using FilterDM.Managers;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using System;

namespace FilterDM.Factories;

public interface IModifiersFactory
{
    ColorDecoratorViewModel BuildColorDecorator(RuleDetailsViewModel ruleDetailsViewModel, PalleteManager palleteManager, Action<ModifierViewModelBase> removeColorModifier);
    T BuildDecorator<T>(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> deleteCallback) where T : ModifierViewModelBase, new();
    MapIconDecoratorViewModel BuildMinimapIconDecorator(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> removeMinimapIconModifier);
    NumericDecoratorViewModel BuildNumericDecorator(RuleDetailsViewModel ruleDetailsViewModel, NumericFilterType numericType);
    SoundDecoratorViewModel BuildSoundDecorator(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> removeSoundModifier);
    TypeDecoratorViewModel BuildTypeDecorator(RuleDetailsViewModel ruleDetailsViewModel, TypeScopeManager typeScopeManager);
}
