using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.Managers;
public partial class PalleteManager : ViewModelBase
{
    [ObservableProperty]
    public ObservableCollection<Color> _usedColors;

    private Dictionary<Color, int> _colorsMap;

    public void Update(Color previous , Color current)
    {
        int c = _colorsMap.GetValueOrDefault(previous, 0);
        c -= 1;
        _colorsMap[previous] = c;
        if (c <= 0)
        {
            UsedColors.Remove(previous);
        }

        c = _colorsMap.GetValueOrDefault(current, 0);
        c += 1;
        _colorsMap[current] = c;
        if (c == 1)
        {
            UsedColors.Add(current);
        }
    }

    public PalleteManager()
    {
        UsedColors = new();
        _colorsMap = new();
    }
}
