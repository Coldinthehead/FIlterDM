using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.EditPage.Decorators;
using System.Data;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class FontSizeEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private TextSizeDecoratorViewModel _decorator;
    public FontSizeEditorViewModel(RuleDetailsViewModel rule, TextSizeDecoratorViewModel decorator) : base(rule)
    {
        _decorator = decorator;
    }
}
     
