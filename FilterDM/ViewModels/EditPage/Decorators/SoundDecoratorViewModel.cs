using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public class SoundDecoratorViewModel : ModifierViewModelBase
{
    public SoundDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }
}
