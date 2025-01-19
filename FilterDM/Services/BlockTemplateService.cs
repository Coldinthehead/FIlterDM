using FilterDM.Models;
using FilterDM.Repositories;
using System;

namespace FilterDM.Services;

public class BlockTemplateService
{
    private readonly IBlockTemplateRepository _repository;

    public BlockTemplateService(IBlockTemplateRepository repository)
    {
        _repository = repository;
    }

    internal BlockModel GetEmpty() => throw new NotImplementedException();
    internal bool TryGetTemplate(string tempalteName, out BlockModel template) => throw new NotImplementedException();
}
