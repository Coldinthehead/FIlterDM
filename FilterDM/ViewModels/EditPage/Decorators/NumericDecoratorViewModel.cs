using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Events;
using System;

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
        _model.UseEquals = value;
        RecalculateRepr();
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }


    [ObservableProperty]
    private int _value;
    partial void OnValueChanged(int value)
    {
        _model.Number = value;
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

    public NumericCondition Model => _model;
    public NumericCondition _model;

    public NumericFilterType FilterType => _type;
    private NumericFilterType _type;

    private NumericFilterHelper _helper;
    public NumericDecoratorViewModel(RuleDetailsViewModel rule
        , NumericCondition model
        , NumericFilterType type
        , NumericFilterHelper helper
        , Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        _helper = helper;
        _type = type;
        _model = model;
        ShortTitle = helper.ShortName;
        LongTitle = helper.Name;
        Value = _model.Number;
        MaxValue = helper.MaxValue;
        Sign = model.UseEquals;
        RecalculateRepr();
    }
}
