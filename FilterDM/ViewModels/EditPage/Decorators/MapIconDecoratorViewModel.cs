using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class MapIconDecoratorViewModel : ModifierViewModelBase
{

    [ObservableProperty]
    private bool _useMinimapIcon;

    [ObservableProperty]
    private ObservableCollection<string> _iconSizes;

    [ObservableProperty]
    private ObservableCollection<string> _colors;

    [ObservableProperty]
    private ObservableCollection<string> _iconShapes;

    [ObservableProperty]
    private string _selectedIconSize;
    partial void OnSelectedIconSizeChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private string _selectedIconColor;
    partial void OnSelectedIconColorChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    

    [ObservableProperty]
    private string _selectedShape;
    partial void OnSelectedShapeChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    partial void OnUseMinimapIconChanged(bool value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private static ObservableCollection<string> _staticColors = new(["Red", "Green", "Blue", "Brown", "White", "Yellow", "Cyan", "Gray", "Orange", "Pink", "Purple"]);


    public MapIconDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        IconSizes = new ObservableCollection<string>(["Small", "Medium", "Large"]);
        IconShapes = new(["Circle", "Diamond", "Hexagon", "Square", "Star", "Triangle", "Cross", "Moon", "Raindrop", "Kite"
            , "Pentagon", "UpsideDownHouse"]);
        Colors = _staticColors;
        SelectedIconColor = Colors[0];
        SelectedIconSize = IconSizes[0];
        SelectedShape = IconShapes[0];
    }

    public override void Apply(RuleModel model)
    {
        model.EnableIcon();
        model.Icon!.Size = SelectedIconSize;
        model.Icon.Shape = SelectedShape;
        model.Icon.Color = SelectedIconColor;
    }
    internal void SetModel(MinimapIconDetails icon)
    {
        SelectedIconSize = icon.Size;
        SelectedIconColor = icon.Color;
        SelectedShape = icon.Shape;
    }
}