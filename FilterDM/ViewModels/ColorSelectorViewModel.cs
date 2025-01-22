using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilterDM.Managers;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Decorators;
using Material.Styles.Themes;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels;

public partial class ColorSelectorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ColorWrapper _currentColor;

    [ObservableProperty]
    private bool _isOpen;

    [ObservableProperty]
    private PalleteManager _palleteManager;

    private Color _startColor;
    partial void OnIsOpenChanged(bool value)
    {
        if (value == true)
        {
            _startColor = CurrentColor.Color;
        }
        else
        {
            PalleteManager.Update(_startColor, CurrentColor.Color);
        }
    }

    [RelayCommand]
    private void ChoseColor(Color color)
    {
        CurrentColor.Color = color;
    }

    public ColorSelectorViewModel(ColorWrapper currentColor, PalleteManager palleteManager)
    {
        CurrentColor = currentColor;
        PalleteManager = palleteManager;
    }
}
