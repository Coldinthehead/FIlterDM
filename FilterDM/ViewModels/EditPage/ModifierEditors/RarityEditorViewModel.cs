using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class RarityEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private RarityDecoratorViewModel _decorator;
    
    public RarityEditorViewModel(RuleDetailsViewModel rule, RarityDecoratorViewModel vm) : base(rule)
    {
        _decorator = vm;
    }
}
