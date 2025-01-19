using FilterDM.Models;
using FilterDM.Services;
using System.Collections.Generic;

namespace FilterDM.Repositories;

public interface IBlockTemplateRepository : IInit
{
    bool Exists(string templateName);
    BlockModel Get(string templateName);
    List<BlockModel> GetAll();
    BlockModel GetEmpty();
}
