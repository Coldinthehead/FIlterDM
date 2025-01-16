using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public class BeamDecoratorViewModel : ModifierViewModelBase
{
    public BeamDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }
}
