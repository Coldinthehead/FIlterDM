using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Factories;

public interface IFilterViewModelFactory
{
    public FilterViewModel BuildFilterViewModel();
}

public class FilterViewModelFactory : IFilterViewModelFactory
{
    public FilterViewModel BuildFilterViewModel()
    {
        FilterViewModel vm;




        return null;
    }

    public BlockDetailsViewModel BuildBlockViewModel()
    {
        return null;
    }

    public RuleDetailsViewModel BuildRuleViewModel()
    {
        return null;
    }
}
