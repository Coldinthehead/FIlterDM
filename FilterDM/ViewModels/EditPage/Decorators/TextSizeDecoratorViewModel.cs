using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using System;


namespace FilterDM.ViewModels.EditPage.Decorators;
public partial class TextSizeDecoratorViewModel : ModifierViewModelBase
{

    [ObservableProperty]
    private bool _useFontSize = false;
    partial void OnUseFontSizeChanged(bool value)
    {
        if (value == true)
        {
            FontSize = _cachedFontSize;
        }
        else
        {
            _cachedFontSize = FontSize;
            FontSize = DEFAULT_FONT_SIZE;
        }
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private static float DEFAULT_FONT_SIZE = 32;
    [ObservableProperty]
    private float _fontSize = DEFAULT_FONT_SIZE;
    partial void OnFontSizeChanged(float oldValue, float newValue)
    {
        Messenger.Send(new FilterEditedRequestEvent(this));
    }

    private float _cachedFontSize = DEFAULT_FONT_SIZE;
    public override void Apply(RuleModel model)
    {
        model.FontSize = (int)FontSize;
    }

    internal void SetModel(RuleModel rule)
    {
        FontSize = rule.FontSize;
    }

    public override ModifierEditorViewModel GetEditor() => new FontSizeEditorViewModel(Rule, this);
}
