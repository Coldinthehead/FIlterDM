using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;
using FilterDM.Views.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class TypeEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private TypeDecoratorViewModel _decorator;
    public TypeEditorViewModel(RuleDetailsViewModel rule, TypeDecoratorViewModel vm) : base(rule)
    {
        _decorator = vm;
    }
}
