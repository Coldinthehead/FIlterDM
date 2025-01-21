using FilterDM.Models;
using System.Threading.Tasks;

namespace FilterDM.Services;
public class ProjectService : IProjectService
{
    public Task Save(FilterModel filterModel) => throw new System.NotImplementedException();
}

public interface IProjectService
{
    Task Save(FilterModel filterModel);
}
