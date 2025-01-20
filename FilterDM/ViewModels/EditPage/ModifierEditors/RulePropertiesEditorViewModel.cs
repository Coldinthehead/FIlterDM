using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class RulePropertiesEditorViewModel : ModifierEditorViewModel
{

    [ObservableProperty]
    private RulePropertiesDecoratorViewModel _decorator;

    public RulePropertiesEditorViewModel(RuleDetailsViewModel rule, RulePropertiesDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
