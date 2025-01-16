using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage;
public partial class DecoratorsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<string> _beamColors;

    [ObservableProperty]
    private string _selectedBeamColor;

    [ObservableProperty]
    private ObservableCollection<string> _iconSizes;

    [ObservableProperty]
    private string _selectedIconSize;
    [ObservableProperty]
    private string _selectedIconColor;

    [ObservableProperty]
    private ObservableCollection<string> _iconShapes;

    [ObservableProperty]
    private string _selectedShape;

    [ObservableProperty]
    private ObservableCollection<int> _sounds;

    [ObservableProperty]
    private int _selectedSound;

    public DecoratorsViewModel()
    {
        BeamColors = new ObservableCollection<string>(["Red", "Green", "Blue", "Brown", "White", "Yellow", "Cyan", "Gray", "Orange", "Pink", "Purple"]);
        SelectedBeamColor = "Red";

        IconSizes = new ObservableCollection<string>(["Small", "Medium", "Large"]);
        SelectedIconSize = "Small";

        SelectedIconColor = "Red";

        IconShapes = new(["Circle", "Diamond", "Hexagon", "Square", "Star", "Triangle", "Cross", "Moon", "Raindrop", "Kite"
            , "Pentagon", "UpsideDownHouse"]);
        SelectedShape = "Circle";

        Sounds = new([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]);

        SelectedSound = 1;
    }

}
