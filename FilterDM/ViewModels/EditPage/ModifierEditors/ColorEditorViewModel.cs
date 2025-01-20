using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class ColorEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private ColorDecoratorViewModel _decorator;
    public ColorEditorViewModel(RuleDetailsViewModel rule, ColorDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
     
