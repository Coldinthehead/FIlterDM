using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilterDM.Services;
using FilterDM.ViewModels.EditPage.Decorators;
using Microsoft.Extensions.DependencyInjection;

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
