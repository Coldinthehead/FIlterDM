using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Events;
using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class ColorDecoratorViewModel : ModifierViewModelBase
{

    [ObservableProperty]
    private bool _useAnyColor;

    [ObservableProperty]
    private bool _useFontColor = false;
    partial void OnUseFontColorChanged(bool value)
    {
        if (value == true)
        {
            TextColor = _cachedFontColor;
            UseAnyColor = true;
        }
        else
        {
            _cachedFontColor = TextColor;
            TextColor = DEFAULT_FONT_COLOR;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private Color _textColor = DEFAULT_FONT_COLOR;
    partial void OnTextColorChanged(Color value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private Color _cachedFontColor = DEFAULT_FONT_COLOR;
    private static Color DEFAULT_FONT_COLOR = new Color(255, 100, 97, 87);


    [ObservableProperty]
    private bool _useBorderColor = false;
    partial void OnUseBorderColorChanged(bool value)
    {
        if (value == true)
        {
            UseAnyColor = true;
            BorderColor = _cachedBorderColor;
        }
        else
        {
            _cachedBorderColor = BorderColor;
            BorderColor = DEFAULT_BORDER_COLOR;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private Color _borderColor = DEFAULT_BORDER_COLOR;
    partial void OnBorderColorChanged(Color value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    private Color _cachedBorderColor = DEFAULT_BORDER_COLOR;

    private static Color DEFAULT_BORDER_COLOR = Colors.Black;


    [ObservableProperty]
    private bool _useBackColor = false;
    partial void OnUseBackColorChanged(bool value)
    {
        if (value == true)
        {
            UseAnyColor = true;
            BackColor = _cachedBackColor;
        }
        else
        {
            _cachedBackColor = BackColor;
            BackColor = DEFAULT_BACK_COLOR;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private Color _backColor = DEFAULT_BACK_COLOR;
    partial void OnBackColorChanged(Color value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

 
    private Color _cachedBackColor = DEFAULT_BACK_COLOR;

    private static Color DEFAULT_BACK_COLOR = Colors.Black;

    public ColorDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }

    internal void SetModel(RuleModel rule)
    {
        if (rule.TryGetTextColor(out Color textColor))
        {
            UseFontColor = true;
            TextColor = textColor;
        }
        else
        {
            UseFontColor = false;
        }

        if (rule.TryGetBorderColor(out Color borderColor))
        {
            UseBorderColor = true;
            BorderColor = borderColor;
        }
        else
        {
            UseBorderColor = false;
        }
        if (rule.TryGetBackgroundColor(out Color backgroundColor))
        {
            UseBackColor = true;
            BackColor = backgroundColor;
        }
        else
        {
            UseBackColor = false;
        }
    }

}