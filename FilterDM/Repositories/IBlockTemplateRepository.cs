using FilterDM.Models;
using FilterDM.Services;

namespace FilterDM.Repositories;

public interface IBlockTemplateRepository : IInit
{
    bool Exists(string templateName);
    BlockModel Get(string templateName);
    BlockModel GetEmpty();
}
