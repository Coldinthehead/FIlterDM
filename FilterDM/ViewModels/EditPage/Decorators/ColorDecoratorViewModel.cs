using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public class ColorDecoratorViewModel : ModifierViewModelBase
{
    public ColorDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }
}