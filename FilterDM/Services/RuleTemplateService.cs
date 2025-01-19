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

    public RuleModel Get(string templateName)
    {
        if (_repository.Has(templateName))
        {
            return _repository.Get(templateName);
        }
        return _repository.GetEmpty();
    }
    public IEnumerable<RuleModel> GetAll() => _repository.GetAll();
    public RuleModel GetEmpty() => _repository.GetEmpty();
}
