using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;
using System.Collections.ObjectModel;


namespace FilterDM.ViewModels;


public partial class ColorSelectorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Color> _pallete;

    [ObservableProperty]
    private Color _color;

    [ObservableProperty]
    private ColorDecoratorViewModel _decorator;

    [ObservableProperty]
    private ColorWrapper _currentColor;

    public ColorSelectorViewModel(ColorDecoratorViewModel decorator, ColorWrapper currentColor)
    {
        _decorator = decorator;
        CurrentColor = currentColor;
        _pallete = new([
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
            Colors.Red,
        ]);
    }
}
