using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Enums;
using FilterDM.Helpers;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.Events;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using System;
using System.Reflection.Metadata.Ecma335;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class NumericDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private string _shortTitle;

    [ObservableProperty]
    private string _longTitle;

    [ObservableProperty]
    private int _maxValue;

    [ObservableProperty]
    private string _valueRepr;

    [ObservableProperty]
    private NumericConditionSign _sign;
    partial void OnSignChanged(NumericConditionSign value)
    {
        RecalculateRepr();
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }


    [ObservableProperty]
    private int _value;
    partial void OnValueChanged(int value)
    {
        RecalculateRepr();
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }

    private void RecalculateRepr()
    {
        switch (Sign)
        {
            case NumericConditionSign.Less:
            ValueRepr = $"{Value}-";
            break;
            case NumericConditionSign.Greater:
            ValueRepr = $"{Value}+";
            break;
            case NumericConditionSign.Eq:
            ValueRepr = $"={Value}";
            break;
        }
    }


    public NumericFilterType FilterType => _type;
    private NumericFilterType _type;
    public NumericDecoratorViewModel(NumericFilterHelper helper)
    {
        _type = helper.Type;
        ShortTitle = helper.ShortName;
        LongTitle = helper.Name;
        MaxValue = helper.MaxValue;
    }

    public override void Apply(RuleModel model)
    {
        NumericCondition condition = model.AddNumericCondition();
        condition.ValueName = _type.ToString();
        condition.Number = Value;
        condition.UseEquals = Sign;
    }
    public void SetModel(NumericCondition model)
    {
        Value = model.Number;
        Sign = model.UseEquals;
        RecalculateRepr();
    }

    public override ModifierEditorViewModel GetEditor() => new NumericEditorViewModel(Rule, this);
}
