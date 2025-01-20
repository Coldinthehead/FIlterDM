using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class BeamEditorViewModel : ModifierEditorViewModel
{

    [ObservableProperty]
    private BeamDecoratorViewModel _decorator;
    public BeamEditorViewModel(RuleDetailsViewModel rule, BeamDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
     
