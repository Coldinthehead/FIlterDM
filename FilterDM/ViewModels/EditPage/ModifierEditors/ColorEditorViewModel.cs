using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Managers;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;

namespace FilterDM.ViewModels.EditPage.ModifierEditors;

public partial class ColorEditorViewModel : ModifierEditorViewModel
{
    [ObservableProperty]
    private ColorDecoratorViewModel _decorator;

    [ObservableProperty]
    private ColorSelectorViewModel _fontColorSelector;

    [ObservableProperty]
    private ColorSelectorViewModel _borderColorSelector;
    [ObservableProperty]
    private ColorSelectorViewModel _backColorSelector;
    public ColorEditorViewModel(RuleDetailsViewModel rule, ColorDecoratorViewModel decorator, PalleteManager palleteManager) : base(rule)
    {
        _decorator = decorator;
        FontColorSelector = new(decorator.TextColor, palleteManager);
        BorderColorSelector = new(decorator.BorderColor, palleteManager);
        BackColorSelector = new(decorator.BackColor, palleteManager);
    }
}
     
