using FilterDM.Managers;
using FilterDM.Services;
using FilterDM.ViewModels;
using FilterDM.ViewModels.EditPage;

namespace FilterDM.Factories;

public interface IFilterViewModelFactory
{
    public FilterViewModel BuildFilterViewModel();
}

public interface IBlockViewModelFactory
{
    public BlockDetailsViewModel BuildBlockViewModel();
}

public class FilterViewModelFactory : IFilterViewModelFactory, IBlockViewModelFactory
{
    private readonly ItemTypeService _itemTypeService;
    private readonly RuleTemplateService _ruleTemplateService;
    private readonly MinimapIconsService _minimapIconService;
    private readonly SoundService _soundService;
    private readonly BlockTemplateManager _blockTemplateManager;

    public FilterViewModelFactory(ItemTypeService itemTypeService
        , RuleTemplateService ruleTemplateService
        , MinimapIconsService minimapIconService
        , SoundService soundService
        , BlockTemplateManager blockTemplateManager)
    {
        _itemTypeService = itemTypeService;
        _ruleTemplateService = ruleTemplateService;
        _minimapIconService = minimapIconService;
        _soundService = soundService;
        _blockTemplateManager = blockTemplateManager;
    }

    public FilterViewModel BuildFilterViewModel()
    {
        FilterViewModel vm = new(_blockTemplateManager
            , new RuleTemplateManager(_ruleTemplateService)
            , new PalleteManager()
            , _minimapIconService
            , _soundService
            , new RuleParentManager()
            , this);
        return vm;
    }

    public BlockDetailsViewModel BuildBlockViewModel()
    {
        BlockDetailsViewModel block = new(_blockTemplateManager, new TypeScopeManager(_itemTypeService));
        return block;
    }

    public RuleDetailsViewModel BuildRuleViewModel()
    {
        return null;
    }
}
