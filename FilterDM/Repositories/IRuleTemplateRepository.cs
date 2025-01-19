using FilterDM.Models;
using FilterDM.Services;
using System.Collections.Generic;

namespace FilterDM.Repositories;

public interface IRuleTemplateRepository : IInit
{
    RuleModel Get(string templateName);
    IEnumerable<RuleModel> GetAll();
    RuleModel GetEmpty();
    bool Has(string templateName);
}
