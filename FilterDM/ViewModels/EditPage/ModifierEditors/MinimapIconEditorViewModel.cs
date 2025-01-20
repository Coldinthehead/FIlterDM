using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class MinimapIconEditorViewModel : ModifierEditorViewModel
{

    [ObservableProperty]
    private MapIconDecoratorViewModel _decorator;
    public MinimapIconEditorViewModel(RuleDetailsViewModel rule, MapIconDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
     
