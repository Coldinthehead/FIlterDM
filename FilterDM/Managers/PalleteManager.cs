using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.Managers;
public partial class PalleteManager : ObservableRecipient, IRecipient<RuleDeleteEvent>
{
    [ObservableProperty]
    public ObservableCollection<Color> _usedColors;

    private Dictionary<Color, int> _colorsMap;

    public void Update(Color previous , Color current)
    {
        int c = _colorsMap.GetValueOrDefault(previous, 0);
        c -= 1;
        c = Math.Max(c, 0);
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

    internal void OnRemove(Color color)
    {
        int c = _colorsMap.GetValueOrDefault(color, 0);
        c -= 1;
        c = Math.Max(c, 0);
        _colorsMap[color] = c;
        if (c <= 0)
        {
            UsedColors.Remove(color);
        }
    }
    internal void OnAdd(Color color)
    {
        int c;
        c = _colorsMap.GetValueOrDefault(color, 0);
        c += 1;
        _colorsMap[color] = c;
        if (c == 1)
        {
            UsedColors.Add(color);
        }
    }

    public void Receive(RuleDeleteEvent message)
    {
        OnRemove(message.Value.Colors.TextColor.Color);
        OnRemove(message.Value.Colors.BorderColor.Color);
        OnRemove(message.Value.Colors.BackColor.Color);
    }

    public PalleteManager()
    {
        UsedColors = new();
        _colorsMap = new();
        Messenger.Register(this);
    }
}
