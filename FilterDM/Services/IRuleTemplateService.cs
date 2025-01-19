using FilterDM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.Services;

public interface IRuleTemplateService
{
    IEnumerable<RuleModel> GetAll();
}
