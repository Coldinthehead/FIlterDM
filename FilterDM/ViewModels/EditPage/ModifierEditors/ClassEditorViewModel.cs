using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Decorators;
using System.Collections.ObjectModel;

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
