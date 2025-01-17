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
        GenerateDescription();
    }

    [ObservableProperty]
    private bool _useMagic;
    partial void OnUseMagicChanged(bool value)
    {
        GenerateDescription();
    }

    [ObservableProperty]
    private bool _useRare;
    partial void OnUseRareChanged(bool oldValue, bool newValue)
    {
        GenerateDescription();
    }

    [ObservableProperty]
    private bool _useUnique;
    partial void OnUseUniqueChanged(bool value)
    {
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


    public RarityDecoratorViewModel(RuleDetailsViewModel rule, Action<ModifierViewModelBase> deleteAction) : base(rule, deleteAction)
    {
        GenerateDescription();
    }

    public override void Apply(RuleModel model)
    {
        RarityConditionModel rarity = model.AddRarityCondition();
        rarity.UseNormal = UseNormal;
        rarity.UseMagic = UseMagic;
        rarity.UseRare = UseRare;
        rarity.UseUnique = UseUnique;   
    }

    public void SetModel(RarityConditionModel model)
    {
        UseNormal = model.UseNormal;
        UseMagic = model.UseMagic;
        UseRare = model.UseRare;
        UseUnique = model.UseUnique;
    }

}
