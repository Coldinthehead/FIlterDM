using System.Collections.Generic;

namespace FilterDM.ViewModels.EditPage.Events;

public struct MultipleRuleDeletedDetails
{
    public readonly BlockDetailsViewModel Block;
    public readonly List<RuleDetailsViewModel> Rules;

    public MultipleRuleDeletedDetails(BlockDetailsViewModel block, List<RuleDetailsViewModel> rules)
    {
        Block = block;
        Rules = rules;
    }
}
