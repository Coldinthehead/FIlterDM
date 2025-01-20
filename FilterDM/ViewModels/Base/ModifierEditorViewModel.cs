using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.ViewModels.Base;

public partial class ModifierEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private RuleDetailsViewModel _rule;
    public ModifierEditorViewModel(RuleDetailsViewModel rule)
    {
        Rule = rule;
    }
}
     
