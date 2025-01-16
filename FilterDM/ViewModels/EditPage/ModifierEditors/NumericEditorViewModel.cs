using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class NumericEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private NumericDecoratorViewModel _decorator;

    public NumericEditorViewModel(RuleDetailsViewModel rule, NumericDecoratorViewModel vm) : base(rule)
    {
        _decorator = vm;
    }
}
