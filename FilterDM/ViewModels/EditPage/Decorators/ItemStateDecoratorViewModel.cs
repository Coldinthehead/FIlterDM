﻿using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.Models;
using FilterDM.ViewModels.Base;
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
    
    public ItemStateDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
    }

    public override void Apply(RuleModel model) => throw new NotImplementedException();
    public override ModifierEditorViewModel GetEditor() => throw new NotImplementedException();
}
