using FilterDM.Enums;
using FilterDM.Managers;
using FilterDM.ViewModels.EditPage;
using FilterDM.ViewModels.EditPage.Decorators;
using System;

namespace FilterDM.Factories;
public class ModifiersFactory : IModifiersFacotry
{
    public ColorDecoratorViewModel BuildColorDecorator(RuleDetailsViewModel ruleDetailsViewModel, PalleteManager palleteManager, Action<ModifierViewModelBase> removeColorModifier) => throw new NotImplementedException();
    public T BuildDecorator<T>(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> deleteCallback) where T : ModifierViewModelBase => throw new NotImplementedException();
    public MapIconDecoratorViewModel BuildMinimapIconDecorator(RuleDetailsViewModel ruleDetailsViewModel, Action<ModifierViewModelBase> removeMinimapIconModifier) => throw new NotImplementedException();
    public NumericDecoratorViewModel BuildNumericDecorator(RuleDetailsViewModel ruleDetailsViewModel, NumericFilterType numericType) => throw new NotImplementedException();
    public TypeDecoratorViewModel BuildTypeDecorator(RuleDetailsViewModel ruleDetailsViewModel, TypeScopeManager typeScopeManager) => throw new NotImplementedException();
}
