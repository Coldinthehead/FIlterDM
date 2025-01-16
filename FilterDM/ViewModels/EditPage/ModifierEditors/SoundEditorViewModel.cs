using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;
public partial class SoundEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private SoundDecoratorViewModel _decorator;
    public SoundEditorViewModel(RuleDetailsViewModel rule, SoundDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
