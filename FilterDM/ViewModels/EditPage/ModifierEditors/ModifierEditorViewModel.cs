using CommunityToolkit.Mvvm.ComponentModel;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class ModifierEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private RuleDetailsViewModel _rule;
    public ModifierEditorViewModel(RuleDetailsViewModel rule)
    {
        Rule = rule;
    }
}
     
