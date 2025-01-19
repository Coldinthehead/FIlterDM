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

    internal BlockModel GetTemplate(string templateName)
    {
        if (_repository.Exists(templateName))
        {
            return _repository.Get(templateName);
        }
        return _repository.GetEmpty();
    }
    internal List<BlockModel> GetTemplates() => _repository.GetAll();
    internal bool HasTemplate(string templateName) => _repository.Exists(templateName);
}
