using FilterDM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.Services;

public interface IRuleTemplateService
{
    RuleModel Get(string templateName);
    IEnumerable<RuleModel> GetAll();
    RuleModel GetEmpty();
}
