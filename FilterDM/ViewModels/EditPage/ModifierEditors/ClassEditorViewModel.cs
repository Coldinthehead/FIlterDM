using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class ClassEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private ClassDecoratorViewModel _decorator;

    public ClassEditorViewModel(RuleDetailsViewModel rule, ClassDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
