using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Decorators;

public partial class RuleParentManager : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<BlockDetailsViewModel> _allBlocks;

    public RuleParentManager(ObservableCollection<BlockDetailsViewModel> allBlocks)
    {
        AllBlocks = allBlocks;
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
    internal bool RequireChange(RuleDetailsViewModel rule, BlockDetailsViewModel selectedParent)
    {
        if (selectedParent != null)
        {
            return !selectedParent.Rules.Contains(rule); 
            
        }
        return false;
    }
}