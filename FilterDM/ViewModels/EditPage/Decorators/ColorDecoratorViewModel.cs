using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using System;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class ColorWrapper : ObservableRecipient
{
    [ObservableProperty]
    private Color _color;

    partial void OnColorChanged(Color value)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    public Color CachedColor;
    public Color DefaultColor;

    public void UseTrue()
    {
        Color = CachedColor;
    }

    public void UseFalse()
    {
        CachedColor = Color;
        Color = DefaultColor;
    }
}

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
            TextColor.UseTrue();
            UseAnyColor = true;
        }
        else
        {
            TextColor.UseFalse();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    [ObservableProperty]
    private ColorWrapper _textColor;
    [ObservableProperty]
    private bool _useBorderColor = false;
    partial void OnUseBorderColorChanged(bool value)
    {
        if (value == true)
        {
            UseAnyColor = true;
            BorderColor.UseTrue();
        }
        else
        {
            BorderColor.UseFalse();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private ColorWrapper _borderColor;

    [ObservableProperty]
    private bool _useBackColor = false;
    partial void OnUseBackColorChanged(bool value)
    {
        if (value == true)
        {
            UseAnyColor = true;
            BackColor.UseTrue();
        }
        else
        {
            BackColor.UseFalse();
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }
    [ObservableProperty]
    private ColorWrapper _backColor;

    private readonly PalleteManager _palleteManager;

    public ColorDecoratorViewModel(
        RuleDetailsViewModel rule,
        PalleteManager palleteManager,
        Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        _palleteManager = palleteManager;
        TextColor = new()
        {
            DefaultColor = new Color(255, 100, 97, 87),
            CachedColor = new Color(255, 100, 97, 87),
        };
        TextColor.Color = TextColor.DefaultColor;

        BorderColor = new()
        {
            DefaultColor = Colors.Black,
            CachedColor = Colors.Black,
        };
        BorderColor.Color = BorderColor.DefaultColor;

        BackColor = new()
        {
            DefaultColor = Colors.Black,
            CachedColor = Colors.Black,
        };
        BackColor.Color = BackColor.DefaultColor;
    }

    public override void Apply(RuleModel model)
    {
        if (UseFontColor)
        {
            model.AddTextColor(TextColor.Color);
        }
        if (UseBackColor)
        {
            model.AddBackgroundColor(BackColor.Color);
        }
        if (UseBorderColor)
        {
            model.AddBorderColor(BorderColor.Color);
        }
    }

    internal void SetModel(RuleModel rule)
    {
        if (rule.TryGetTextColor(out Color textColor))
        {
            UseFontColor = true;
            TextColor.Color = textColor;
        }
        else
        {
            UseFontColor = false;
        }

        if (rule.TryGetBorderColor(out Color borderColor))
        {
            UseBorderColor = true;
            BorderColor.Color = borderColor;
        }
        else
        {
            UseBorderColor = false;
        }
        if (rule.TryGetBackgroundColor(out Color backgroundColor))
        {
            UseBackColor = true;
            BackColor.Color = backgroundColor;
        }
        else
        {
            UseBackColor = false;
        }
    }

    public override ModifierEditorViewModel GetEditor() => new ColorEditorViewModel(Rule,this, _palleteManager);
}