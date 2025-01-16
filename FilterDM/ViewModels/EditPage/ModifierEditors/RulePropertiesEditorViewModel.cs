using Avalonia;
using CommunityToolkit.Mvvm.Input;
using FilterDM.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class RulePropertiesEditorViewModel : ModifierEditorViewModel
{



    public RulePropertiesEditorViewModel(RuleDetailsViewModel rule) : base(rule)
    {
    }
}
