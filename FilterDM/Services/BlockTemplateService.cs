using FilterDM.Repositories;

namespace FilterDM.Services;

public class BlockTemplateService
{
    private readonly IBlockTemplateRepository _repository;

    public BlockTemplateService(IBlockTemplateRepository repository)
    {
        _repository = repository;
    }
}
