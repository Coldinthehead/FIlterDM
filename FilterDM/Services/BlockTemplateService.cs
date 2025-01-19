using FilterDM.Models;
using FilterDM.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FilterDM.Services;

public class BlockTemplateService
{
    private readonly IBlockTemplateRepository _repository;

    public BlockTemplateService(IBlockTemplateRepository repository)
    {
        _repository = repository;
    }

    public BlockModel GetEmpty()
    {
        return _repository.GetEmpty();
    }
    public bool TryGetTemplate(string templateName, out BlockModel template)
    {
        if (_repository.Exists(templateName))
        {
            template = _repository.Get(templateName);
            return true;
        }
        template = null;
        return false;
    }

    internal List<BlockModel> GetTemplates() => throw new NotImplementedException();
}
