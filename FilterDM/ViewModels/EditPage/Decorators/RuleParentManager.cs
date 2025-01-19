using System;
using System.Collections.ObjectModel;

namespace FilterDM.ViewModels.EditPage.Decorators;

public class RuleParentManager
{
    public ObservableCollection<BlockDetailsViewModel> _allBlocks;

    internal void ChangeParent(RuleDetailsViewModel rule, string selectedParent) => throw new NotImplementedException();
    internal string GetMyParentName(RuleDetailsViewModel rule) => throw new NotImplementedException();
    internal ObservableCollection<string> GetObservableNames() => throw new NotImplementedException();
    internal bool RequireChange(RuleDetailsViewModel rulePropertiesDecoratorViewModel, string selectedParent) => throw new NotImplementedException();
}