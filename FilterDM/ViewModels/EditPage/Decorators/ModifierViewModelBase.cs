using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public abstract partial class ModifierViewModelBase : ObservableRecipient
{
    public Action<ModifierViewModelBase> DeleteAction;

    [ObservableProperty]
    private RuleDetailsViewModel _rule;
    [RelayCommand]
    private void DeleteMe()
    {
        DeleteCallback();
    }

    protected virtual void DeleteCallback()
    {
        DeleteAction?.Invoke(this);
    }

    protected ModifierViewModelBase(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction)
    {
        DeleteAction = deleteAction;
        Rule = rule;
    }
}
