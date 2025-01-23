using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Constants;
using FilterDM.Models;
using FilterDM.Services;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
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
    private CroppedBitmap _currentIconImage;

    [ObservableProperty]
    private string _selectedIconSize;
    partial void OnSelectedIconSizeChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
        _currentSizeIndex = IconSizes.IndexOf(value);
        UpdateImage();
    }
    [ObservableProperty]
    private string _selectedIconColor;
    partial void OnSelectedIconColorChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
        _currentColorIndex = Colors.IndexOf(value);
        UpdateImage();
    }

    [ObservableProperty]
    private string _selectedShape;
    partial void OnSelectedShapeChanged(string value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
        _currentShapeIndex = IconShapes.IndexOf(value);
        UpdateImage();
    }

    partial void OnUseMinimapIconChanged(bool value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private static ObservableCollection<string> _staticColors = new([
        ColorConstants.BLUE,
        ColorConstants.GREEN,
        ColorConstants.BROWN,
        ColorConstants.RED,
        ColorConstants.WHITE,
        ColorConstants.YELLOW,
        ColorConstants.CYAN,
        ColorConstants.GRAY,
        ColorConstants.ORANGE,
        ColorConstants.PINK,
        ColorConstants.PURPLE]);

    private static ObservableCollection<string> _staticSizes = new([
        IconSize.LARGE,
        IconSize.MEDIUM,
        IconSize.SMALL
        ]);
    private static ObservableCollection<string> _staticShapes = new([
      IconShapeConstants.CIRCLE,
        IconShapeConstants.DIAMOND,
        IconShapeConstants.HEXAGON,
        IconShapeConstants.SQUARE,
        IconShapeConstants.STAR,
        IconShapeConstants.TRIANGLE,
        IconShapeConstants.CROSS,
        IconShapeConstants.MOON,
        IconShapeConstants.RAINDROP,
        IconShapeConstants.KITE,
        IconShapeConstants.PENTAGON,
        IconShapeConstants.HOUSE,]);

    private int _currentSizeIndex;
    private int _currentShapeIndex;
    private int _currentColorIndex;
    [RelayCommand]
    private void UpdateSize(bool value)
    {
        if (value == true)
        {
            _currentSizeIndex++;
            _currentSizeIndex %= IconSizes.Count;
        }
        else
        {
            _currentSizeIndex--;
            _currentSizeIndex = _currentSizeIndex < 0 ? IconSizes.Count - 1 : _currentSizeIndex;
        }
        SelectedIconSize = IconSizes[_currentSizeIndex];
    }

    [RelayCommand]
    private void UpdateShape(bool value)
    {
        if (value == true)
        {
            _currentShapeIndex++;
            _currentShapeIndex %= IconShapes.Count;
        }
        else
        {
            _currentShapeIndex--;
            _currentShapeIndex = _currentShapeIndex < 0 ? IconShapes.Count - 1 : _currentShapeIndex;
        }
        SelectedShape = IconShapes[_currentShapeIndex];
    }
    [RelayCommand]
    private void UpdateColor(bool value)
    {
        if (value == true)
        {
            _currentColorIndex++;
            _currentColorIndex %= Colors.Count;
        }
        else
        {
            _currentColorIndex--;
            _currentColorIndex = _currentColorIndex < 0 ? Colors.Count - 1 : _currentColorIndex;
        }
        SelectedIconColor = Colors[_currentColorIndex];
    }

    private void UpdateImage()
    {
        CurrentIconImage = _iconService.Get(SelectedIconSize, SelectedShape, Colors.IndexOf(SelectedIconColor));
    }

    private readonly MinimapIconsService _iconService;

    public MapIconDecoratorViewModel(RuleDetailsViewModel rule
        , MinimapIconsService iconService
        , Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        IconSizes = _staticSizes;
        IconShapes = _staticShapes;
        Colors = _staticColors;
        _currentColorIndex = 0;
        _currentShapeIndex = 0;
        _currentColorIndex = 0;
        _selectedIconColor = Colors[0];
        _selectedShape = IconShapes[0];
        _selectedIconSize = IconSizes[0];
        SelectedIconColor = Colors[_currentColorIndex];
        SelectedIconSize = IconSizes[_currentSizeIndex];
        SelectedShape = IconShapes[_currentShapeIndex];
        _iconService = iconService;

        UpdateImage();
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

    public override ModifierEditorViewModel GetEditor() => new MinimapIconEditorViewModel(Rule, this);
}