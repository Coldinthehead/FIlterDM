using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage.Events;
using System;
using System.Collections.Generic;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class RarityDecoratorViewModel : ModifierViewModelBase
{
    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private bool _useNormal;
    partial void OnUseNormalChanged(bool value)
    {
        _model.UseNormal = value;
        GenerateDescription();
    }

    [ObservableProperty]
    private bool _useMagic;
    partial void OnUseMagicChanged(bool value)
    {
        _model.UseMagic = value;
        GenerateDescription();
    }

    [ObservableProperty]
    private bool _useRare;
    partial void OnUseRareChanged(bool oldValue, bool newValue)
    {
        _model.UseRare = newValue;
        GenerateDescription();
    }

    [ObservableProperty]
    private bool _useUnique;
    partial void OnUseUniqueChanged(bool value)
    {
        _model.UseUnique = value;
        GenerateDescription();
    }

    private void GenerateDescription()
    {
        if (UseNormal == UseMagic && UseMagic == UseRare && UseRare == UseUnique)
        {
            Description = "Any";
        }
        else
        {
            List<char> items = [];
            if (UseNormal)
            {
                items.Add('N'); 
            }
            if (UseMagic)
            {
                items.Add('M');
            }
            if (UseRare)
            {
                items.Add('R');
            }
            if (UseUnique)
            {
                items.Add('U');
            }
            Description = string.Join(',', items.ToArray());
        }
        WeakReferenceMessenger.Default.Send(new FilterEditedRequestEvent(this));
    }

    public RarityConditionModel Model => _model;
    private RarityConditionModel _model;

    public RarityDecoratorViewModel(RuleDetailsViewModel rule, RarityConditionModel rarityModel, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        _model = rarityModel;
        UseNormal = _model.UseNormal;
        UseMagic = _model.UseMagic;
        UseRare = _model.UseRare;
        UseUnique = _model.UseUnique;
        GenerateDescription();
    }
}
