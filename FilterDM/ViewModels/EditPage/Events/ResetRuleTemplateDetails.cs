using FilterDM.Models;

namespace FilterDM.ViewModels.EditPage.Events;

public struct ResetRuleTemplateDetails
{
    public readonly RuleDetailsViewModel Rule;
    public readonly RuleModel Template;

    public ResetRuleTemplateDetails(RuleDetailsViewModel rule, RuleModel template)
    {
        Rule = rule;
        Template = template;
    }
}


