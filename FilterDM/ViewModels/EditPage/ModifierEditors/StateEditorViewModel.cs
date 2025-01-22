using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;
public partial class StateEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private ItemStateDecoratorViewModel _decorator;
    public StateEditorViewModel(RuleDetailsViewModel rule, ItemStateDecoratorViewModel decorator) : base(rule)
    {
        Decorator = decorator;
    }
}
