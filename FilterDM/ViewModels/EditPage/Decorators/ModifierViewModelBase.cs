using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public abstract partial class ModifierViewModelBase : ObservableRecipient
{
    public Action<ModifierViewModelBase> DeleteAction { get; set; }

    [ObservableProperty]
    private RuleDetailsViewModel _rule;
    [RelayCommand]
    private void DeleteMe()
    {
        DeleteCallback();
    }


    public abstract void Apply(RuleModel model);

    protected virtual void DeleteCallback()
    {
        DeleteAction?.Invoke(this);
    }

    public abstract ModifierEditorViewModel GetEditor();

}
