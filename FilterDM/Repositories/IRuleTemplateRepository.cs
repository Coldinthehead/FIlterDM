using FilterDM.Models;
using FilterDM.Services;
using System.Collections.Generic;

namespace FilterDM.Repositories;

public interface IRuleTemplateRepository : IInit
{
    IEnumerable<RuleModel> GetAll();
}
