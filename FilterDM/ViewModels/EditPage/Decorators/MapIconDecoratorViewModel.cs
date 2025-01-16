using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public class MapIconDecoratorViewModel : ModifierViewModelBase
{
    public MapIconDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }
}