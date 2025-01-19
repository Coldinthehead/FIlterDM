using FilterDM.Models;
using FilterDM.Repositories;
using System.Collections.Generic;

namespace FilterDM.Services;
public class RuleTemplateService : IRuleTemplateService
{
    private readonly IRuleTemplateRepository _repository;

    public RuleTemplateService(IRuleTemplateRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<RuleModel> GetAll() => _repository.GetAll();
}
