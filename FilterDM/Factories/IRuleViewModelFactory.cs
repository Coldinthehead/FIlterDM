using FilterDM.Managers;
using FilterDM.Models;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Factories;

public interface IRuleViewModelFactory
{
    public RuleDetailsViewModel BuildRuleViewModel(BlockDetailsViewModel parent, RuleParentManager parentManager, PalleteManager pallateManager);
    public RuleDetailsViewModel BuildRuleViewModel(BlockDetailsViewModel parent, RuleParentManager parentManager, PalleteManager pallateManager, RuleModel model);
}
