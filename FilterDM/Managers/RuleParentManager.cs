﻿using CommunityToolkit.Mvvm.ComponentModel;
using FilterDM.ViewModels.Base;
using FilterDM.ViewModels.EditPage;
using System;
using System.Collections.ObjectModel;

namespace FilterDM.Managers;

public partial class RuleParentManager : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _allBlocks;

    public RuleParentManager()
    {
        AllBlocks = new();
    }

    public void SetBlocks(ObservableCollection<BlockDetailsViewModel> next)
    {
        AllBlocks = next;
    }

    internal void ChangeParent(RuleDetailsViewModel rule, BlockDetailsViewModel selectedParent)
    {
        foreach (var block in AllBlocks)
        {
            if (block.Rules.Contains(rule))
            {
                block.Rules.Remove(rule);
                break;
            }
        }

        selectedParent.AddRule(rule);
    }

    internal bool DeleteRule(RuleDetailsViewModel rule)
    {
        foreach (BlockDetailsViewModel block in AllBlocks)
        {
            if (block.Rules.Contains(rule))
            {
                block.DeleteRule(rule);
                return true;
            }
        }
        return false;
    }
    internal bool RemoveBlock(BlockDetailsViewModel vm) => AllBlocks.Remove(vm);

    internal bool RequireChange(RuleDetailsViewModel rule, BlockDetailsViewModel selectedParent)
    {
        if (selectedParent != null)
        {
            return !selectedParent.Rules.Contains(rule); 
            
        }
        return false;
    }
}
