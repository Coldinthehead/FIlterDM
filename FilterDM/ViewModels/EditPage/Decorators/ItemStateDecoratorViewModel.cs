using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage.ModifierEditors;
using System;
using System.Collections.Generic;


namespace FilterDM.ViewModels.EditPage.Decorators;
public partial class ItemStateDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private bool _corrupted;
    partial void OnCorruptedChanging(bool value)
    {
        UpdateRepr();
    }

    [ObservableProperty]
    private bool _mirrored;
    partial void OnMirroredChanged(bool value)
    {
        UpdateRepr();
    }

    [ObservableProperty]
    private string _repr;

    private void UpdateRepr()
    {
        List<string> next = [];
        if (Corrupted)
        {
            next.Add("C");
        }
        if (Mirrored)
        {
            next.Add("M");
        }
        Repr = string.Join(",", next);
    }
    
    public override void Apply(RuleModel model)
    {
        model.AddStateModifiers(Mirrored, Corrupted);
    }
    public override ModifierEditorViewModel GetEditor() => new StateEditorViewModel(Rule, this);
    internal void SetModel(StateModifiers stateModifiers)
    {
        Mirrored = stateModifiers.Mirrored;
        Corrupted = stateModifiers.Corrupted;
    }
}
